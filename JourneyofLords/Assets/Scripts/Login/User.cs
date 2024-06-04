using Firebase.Firestore;

[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string email { get; set; }

    [FirestoreProperty]
    public string IGN { get; set; }

    [FirestoreProperty]
    public int gold { get; set; }

    [FirestoreProperty]
    public int userLV { get; set; }

    [FirestoreProperty]
    public int userEXP { get; set; }

    [FirestoreProperty]
    public int wood { get; set; }

    [FirestoreProperty]
    public int stone { get; set; }

    [FirestoreProperty]
    public int iron { get; set; }

    [FirestoreProperty]
    public int food { get; set; }

    [FirestoreProperty]
    public int max_Stage { get; set; }

    [FirestoreProperty]
    public int max_Rewards { get; set; }

    [FirestoreProperty]
    public int maxHeroes { get; set; }

    [FirestoreProperty]
    public int money { get; set; }

    [FirestoreProperty]
    public int troops { get; set; }

    [FirestoreProperty]
    public string userID { get; set; }

    [FirestoreProperty]
    public int energy { get; set; }

    [FirestoreProperty]
    public long maxEnergy { get; set; }
}
