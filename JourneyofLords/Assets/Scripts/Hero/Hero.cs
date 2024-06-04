using System;
using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Hero
{
    [FirestoreProperty]
    public string id { get; set; }

    [FirestoreProperty]
    public int index { get; set; }

    [FirestoreProperty]
    public int grade { get; set; }

    [FirestoreProperty]
    public int rarity { get; set; }

    [FirestoreProperty]
    public string att { get; set; }

    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public string sex { get; set; }
    
    [FirestoreProperty]
    public int level { get; set; }

    [FirestoreProperty]
    public int exp { get; set; }

    [FirestoreProperty]
    public int rebirth { get; set; }

    [FirestoreProperty]
    public int growth { get; set; }
    
    [FirestoreProperty]
    public float atk { get; set; }

    [FirestoreProperty]
    public float def { get; set; }

    [FirestoreProperty]
    public float hp { get; set; }

    [FirestoreProperty]
    public string spriteName { get; set; }

    [FirestoreProperty]
    public List<string> equip { get; set; }

    [FirestoreProperty]
    public string description { get; set; }
}
