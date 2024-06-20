using System;
using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Equip
{
    [FirestoreProperty]
    public string id { get; set; }

    [FirestoreProperty]
    public int index { get; set; }

    [FirestoreProperty]
    public string type { get; set; }

    [FirestoreProperty]
    public int rarity { get; set; }

    [FirestoreProperty]
    public string name { get; set; }
    
    [FirestoreProperty]
    public int level { get; set; }
    
    [FirestoreProperty]
    public float atk { get; set; }

    [FirestoreProperty]
    public float def { get; set; }

    [FirestoreProperty]
    public float hp { get; set; }

    [FirestoreProperty]
    public string spriteName { get; set; }

    [FirestoreProperty]
    public string description { get; set; }
}
