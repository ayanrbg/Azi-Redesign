using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardManager", menuName = "Azi/Card Manager")]
public class CardManager : ScriptableObject
{
    [System.Serializable]
    public class CardSprite
    {
        public string id;      // например "6H", "KD", "AS"
        public Sprite sprite;
    }

    public List<CardSprite> cards = new List<CardSprite>();
    private Dictionary<string, Sprite> cardDict;

    public void Init()
    {
        cardDict = new Dictionary<string, Sprite>();
        foreach (var c in cards)
        {
            if (!cardDict.ContainsKey(c.id))
                cardDict.Add(c.id, c.sprite);
        }
    }

    public Sprite GetCardSprite(string id)
    {
        if (cardDict == null)
            Init();

        if (cardDict.TryGetValue(id, out var sprite))
            return sprite;

        Debug.LogWarning($"? Не найден спрайт для карты: {id}");
        return null;
    }
}
