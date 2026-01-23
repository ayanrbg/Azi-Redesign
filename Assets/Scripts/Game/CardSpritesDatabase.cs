using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CardSpritesDatabase",
    menuName = "Cards/Card Sprites Database"
)]
public class CardSpritesDatabase : ScriptableObject
{
    [Serializable]
    public struct CardEntry
    {
        public string code;   // 6H, KH, QD, JC, AD
        public Sprite sprite;
    }

    [SerializeField]
    private List<CardEntry> cards = new List<CardEntry>();

    private Dictionary<string, Sprite> lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, Sprite>(cards.Count);

        foreach (var card in cards)
        {
            if (string.IsNullOrEmpty(card.code))
                continue;

            if (!lookup.ContainsKey(card.code))
                lookup.Add(card.code, card.sprite);
        }
    }

    public Sprite GetSprite(string code)
    {
        if (lookup == null || lookup.Count == 0)
            BuildLookup();

        if (lookup.TryGetValue(code, out Sprite sprite))
            return sprite;

        Debug.LogWarning($"Card sprite not found for code: {code}");
        return null;
    }
}
