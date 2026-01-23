using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void HandleAuthResult(AuthResultResponse authResult)
    {
        if(authResult.success == true)
        {
            LoadingManager.Instance.LoadMainScene();
        }
    }
}
