﻿using UnityEngine;

public enum ItemType { Default, Food, Weapon, Instrument }
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public int maximumAmount;
    public GameObject itemPrefab;
    public Sprite icon;
    public ItemType itemType;
    public string itemDescription;
    public bool isConsumeable;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;
}