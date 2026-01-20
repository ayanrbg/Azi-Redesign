using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Register")]
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private TMP_InputField loginRegisterInputField;
    [SerializeField] private TMP_InputField passwordRegisterInputField;
    [SerializeField] private TMP_InputField confirmPasswordRegisterInputField;
    [SerializeField] private TextMeshProUGUI errorPass;
    [SerializeField] private TextMeshProUGUI errorLogin;
    [Header("Login")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField loginInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI errorCorrect;
    private bool socketReady = false;
    void Start()
    {
        WebSocketManager.Instance.OnConnected += OnSocketReady;


    }
    private void OnEnable()
    {
        EventBus.OnLoginFailed += ShowLoginFailed;
        EventBus.OnRegisterFailed += ShowRegisterFailed;
    }
    private void OnDisable()
    {
        EventBus.OnLoginFailed -= ShowLoginFailed;
        EventBus.OnRegisterFailed -= ShowRegisterFailed;
    }
    void OnSocketReady()
    {
        socketReady = true;   
        TryAutoLogin();
    }
    private void TryAutoLogin()
    {
        if (!PlayerPrefs.HasKey("login") || !PlayerPrefs.HasKey("password"))
            return;

        string savedLogin = PlayerPrefs.GetString("login");
        string savedPassword = PlayerPrefs.GetString("password");

        if (string.IsNullOrEmpty(savedLogin) || string.IsNullOrEmpty(savedPassword))
            return;

        Debug.Log("AUTO LOGIN");

        // опционально — показать логин-панель
        OpenLoginPanel();


        WebSocketManager.Instance.SendLogin(savedLogin, savedPassword);
    }

    private void ShowLoginFailed(LoginFailedResponse response)
    {
        errorCorrect.gameObject.SetActive(true);
        errorCorrect.text = "*" + response.message;
    }
    public void OpenLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    public void OpenRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
    private void ShowRegisterFailed(RegisterFailedResponse response)
    {
        switch (response.message)
        {
            case "Данный логин занят":
                errorLogin.gameObject.SetActive(true);
                errorLogin.text = response.message;
                break;
            default:
                errorPass.gameObject.SetActive(true);
                errorPass.text = response.message;
                break;
        }
    }
    public void SendLogin()
    {
        if (!socketReady) return;
        errorLogin.gameObject.SetActive(false);
        errorPass.gameObject.SetActive(false);
        errorCorrect.gameObject.SetActive(false);
        WebSocketManager.Instance.SendLogin(loginInputField.text, passwordInputField.text);
    }
    public void SendRegister()
    {
        if (!socketReady) return;
        errorLogin.gameObject.SetActive(false);
        errorPass.gameObject.SetActive(false);
        errorCorrect.gameObject.SetActive(false);
        WebSocketManager.Instance.SendRegister(loginRegisterInputField.text, passwordRegisterInputField.text,
            confirmPasswordRegisterInputField.text);
    }
}
