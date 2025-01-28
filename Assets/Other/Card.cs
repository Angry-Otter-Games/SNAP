using UnityEngine;
using System;

[Serializable]
public class SnapMap {
    public Texture texture;
    public float speed;
    public float strength;

    public bool isValid(){
        return texture;
    }
}

[Serializable]
public class SnapSprite {
    public Sprite sprite;
    public bool frameBreak;
    public float animationZ;
    public int orderInLayer;

    public bool isValid(){
        return sprite;
    }
}

[Serializable]
public enum Rarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Ultra,
    Infinity
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public SnapSprite characterBase;
    public SnapMap characterBaseMap;
    public SnapSprite characterAddon;
    public SnapMap characterAddonMap;
    public SnapSprite title;
    public SnapSprite background;
    public SnapSprite effect1;
    public SnapSprite effect2;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public Rarity rarity;
    public int cost;
    public int power; 
}
