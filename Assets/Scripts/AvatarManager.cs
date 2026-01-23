using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
        }
    }

    public Sprite[] avatarSprites26x26;
    public Sprite[] avatarSprites46x46;
    public Sprite[] avatarSprites152x152;
}
