using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class AuthService : MonoBehaviour
{
    public static AuthService Instance;

    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] TMP_InputField loginField;
    [SerializeField] TMP_InputField lPasswordField;
    [SerializeField] TMP_InputField registerLoginField;
    [SerializeField] TMP_InputField rPasswordField1;
    [SerializeField] TMP_InputField rPasswordField2;
    [SerializeField] TextMeshProUGUI statusText;

    private const string BASE_URL = "http://92.53.124.96:51800/api/auth";

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("token"))
        {
            //GameState.Instance.UpdateToken(PlayerPrefs.GetString("token"));
            WebSocketManager.Instance.ConnectWithAuth();
        }
        
    }

    // ================= LOGIN =================
    public void Login()
    {
        statusText.text = "";

        StartCoroutine(LoginCoroutine(loginField.text, lPasswordField.text));
    }
    public void OpenRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
    public void OpenLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    IEnumerator LoginCoroutine(string email, string password)
    {
        string url = $"{BASE_URL}/login";

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);
            yield break;
        }

        HandleAuthResponse(req.downloadHandler.text);
    }

    // ================= REGISTER =================
    public void Register()
    {
        if(rPasswordField1 != rPasswordField2)
        {
            statusText.text = "Пароли не совпадают";
            return;
        }

        statusText.text = "";
        StartCoroutine(RegisterCoroutine(registerLoginField.text, 
            "{{$randomUserName}}", rPasswordField1.text));
    }

    IEnumerator RegisterCoroutine(string name, string email, string password)
    {
        string url = $"{BASE_URL}/register";

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);
            yield break;
        }

        HandleAuthResponse(req.downloadHandler.text);
    }

    // ================= RESPONSE =================
    void HandleAuthResponse(string json)
    {
        if (json.Contains("\"token\""))
        {
            TokenResponse res = JsonUtility.FromJson<TokenResponse>(json);

            //GameState.Instance.UpdateToken(res.token);

            PlayerPrefs.SetString("token", res.token);
            PlayerPrefs.Save();

            WebSocketManager.Instance.ConnectWithAuth();
        }
        else
        {
            Debug.LogError("Auth error: " + json);

            //var msg = JsonUtility.FromJson<AuthFailedResponse>(json);
            //EventBus.RaiseAuthFailed(msg);
        }
    }
}

[System.Serializable]
public class TokenResponse
{
    public string token;
}
