using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private LoadingUI loadingUI;

    private bool isLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ===================== PUBLIC API =====================

    public void LoadRoomScene() => LoadScene("Rooms");
    public void LoadGameScene() => LoadScene("Game");
    public void LoadMainScene() => LoadScene("Main");
    public void LoadAuthScene() => LoadScene("Auth");

    // ===================== CORE =====================

    private void LoadScene(string sceneName)
    {
        if (isLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        loadingUI.Show();
        loadingUI.SetProgress(0f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float progress = asyncLoad.progress / 0.9f;
            loadingUI.SetProgress(progress);
            yield return null;
        }

        // 🔹 100%
        loadingUI.SetProgress(1f);

        yield return new WaitForSeconds(0.15f);

        asyncLoad.allowSceneActivation = true;

        yield return null;

        loadingUI.Hide();
        isLoading = false;
    }
}
