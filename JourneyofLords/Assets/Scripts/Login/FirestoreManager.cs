using System;
using System.Collections.Generic;
using System.Linq; // System.Linq 추가
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class FirestoreManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseFirestore firestore;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Initialized Successfully");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void AddHero(Hero hero)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User is not authenticated");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DocumentReference heroRef = firestore.Collection("users").Document(userId).Collection("heroes").Document(hero.id);

        Dictionary<string, object> heroData = new Dictionary<string, object>
        {
            { "id", hero.id },
            { "index", hero.index },
            { "grade", hero.grade },
            { "rarity", hero.rarity },
            { "att", hero.att },
            { "name", hero.name },
            { "sex", hero.sex },
            { "level", hero.level },
            { "exp", hero.exp },
            { "rebirth", hero.rebirth },
            { "growth", hero.growth },
            { "atk", hero.atk },
            { "def", hero.def },
            { "hp", hero.hp },
            { "lead", hero.lead },
            { "spriteName", hero.spriteName },
            { "equip", hero.equip },
            { "description", hero.description }
        };

        heroRef.SetAsync(heroData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Hero added to Firestore successfully.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add hero to Firestore: " + task.Exception);
            }
        });
    }

    public async Task<HeroList> GetHeroesData()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User is not authenticated");
            return null;
        }

        string userId = auth.CurrentUser.UserId;
        try
        {
            QuerySnapshot snapshot = await firestore.Collection("users").Document(userId).Collection("heroes").GetSnapshotAsync();

            List<Hero> heroes = new List<Hero>();
            foreach (DocumentSnapshot heroDocument in snapshot.Documents)
            {
                try
                {
                    Hero hero = heroDocument.ConvertTo<Hero>();
                    heroes.Add(hero);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error converting document to Hero: " + e.Message);
                }
            }

            HeroList heroList = ScriptableObject.CreateInstance<HeroList>();
            heroList.heroes = heroes;

            return heroList;
        }
        catch (Exception e)
        {
            Debug.LogError("Error getting heroes data: " + e.Message);
            return null;
        }
    }

    public void UpdateUserMaxHeroes(string userId, int newMaxHeroes)
    {
        DocumentReference docRef = firestore.Collection("users").Document(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "maxHeroes", newMaxHeroes }
        };

        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("maxHeroes updated successfully in Firestore.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error updating maxHeroes: " + task.Exception);
            }
        });
    }

    private void GetHeroItems(string userId, Hero hero)
    {
        List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
        foreach (string itemId in hero.equip)
        {
            firestore.Collection("users").Document(userId).Collection("items").Document(itemId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error getting item data: " + task.Exception);
                    return;
                }

                DocumentSnapshot itemDocument = task.Result;
                items.Add(itemDocument.ToDictionary());
                if (items.Count == hero.equip.Count)
                {
                    // Hero 클래스에 SetItems 메서드가 없으므로, items 데이터를 사용할 곳에서 처리하세요.
                    // 예: hero.SetItems(items);
                }
            });
        }
    }

    public void UpdateUserMaxStage(string userId, int newMaxStage)
    {
        DocumentReference docRef = firestore.Collection("users").Document(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "max_Stage", newMaxStage }
        };

        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("max_Stage updated successfully in Firestore.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error updating max_Stage: " + task.Exception);
            }
        });
    }

    public void GetUserMaxStage(string userId, Action<int> onMaxStageLoaded)
    {
        DocumentReference docRef = firestore.Collection("users").Document(userId);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int maxStage = snapshot.GetValue<int>("max_Stage");
                    onMaxStageLoaded(maxStage);
                }
                else
                {
                    Debug.LogError("User document does not exist.");
                }
            }
            else
            {
                Debug.LogError("Error getting max_Stage: " + task.Exception);
            }
        });
    }

    public void UpdateUserData(User user, Action onSuccess = null, Action<string> onFailure = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User is not authenticated");
            onFailure?.Invoke("User is not authenticated");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DocumentReference userRef = firestore.Collection("users").Document(userId);

        userRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User data updated successfully.");
                onSuccess?.Invoke();
            }
            else
            {
                Debug.LogError("Error updating user data: " + task.Exception);
                onFailure?.Invoke(task.Exception.Message);
            }
        });
    }

    public void GetUserData(string userId, Action<User> onSuccess, Action<string> onFailure)
    {
        DocumentReference userRef = firestore.Collection("users").Document(userId);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    User user = snapshot.ConvertTo<User>();
                    onSuccess?.Invoke(user);
                }
                else
                {
                    Debug.LogError("User data not found.");
                    onFailure?.Invoke("User data not found");
                }
            }
            else
            {
                Debug.LogError("Error getting user data: " + task.Exception);
                onFailure?.Invoke(task.Exception.Message);
            }
        });
    }
}
