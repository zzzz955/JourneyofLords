using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private ListenerRegistration userListener;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public async void SignUp(string email, string password, Action<string> onSignUpSuccess, Action<string> onSignUpFail)
    {
        try
        {
            FirebaseUser newUser = (await auth.CreateUserWithEmailAndPasswordAsync(email, password)).User;
            Debug.Log("User signed up successfully: " + newUser.Email);
            await SaveUserDataToFirestore(newUser);
            onSignUpSuccess?.Invoke(newUser.Email);
        }
        catch (Exception e)
        {
            Debug.LogError("Sign up error: " + e.Message);
            onSignUpFail?.Invoke(e.Message);
        }
    }

    private async Task SaveUserDataToFirestore(FirebaseUser newUser)
    {
        string userDataPath = Path.Combine(Application.dataPath, "Scripts/GameData/UserData.xlsx");
        userDataPath = userDataPath.Replace("\\", "/");
        Debug.Log("Saving user data to path: " + userDataPath);

        try
        {
            List<User> users = GameManager.LoadUserData(userDataPath);
            if (users == null || users.Count == 0)
            {
                Debug.LogError("Failed to load user data.");
                return;
            }

            string ign = "u" + DateTime.Now.ToString("yyMMddHHmmssFFFFFFF");

            User defaultUser = users[0];
            User newUserEntry = new User
            {
                email = newUser.Email,
                IGN = ign,
                gold = defaultUser.gold,
                userLV = defaultUser.userLV,
                userEXP = defaultUser.userEXP,
                wood = defaultUser.wood,
                stone = defaultUser.stone,
                iron = defaultUser.iron,
                food = defaultUser.food,
                max_Stage = defaultUser.max_Stage,
                max_Rewards = defaultUser.max_Rewards,
                maxHeroes = defaultUser.maxHeroes,
                money = defaultUser.money,
                troops = defaultUser.troops,
                userID = newUser.UserId
            };

            DocumentReference userRef = db.Collection("users").Document(newUser.UserId);
            await userRef.SetAsync(newUserEntry);
            Debug.Log("Successfully saved user data to Firestore.");
        }
        catch (Exception e)
        {
            Debug.LogError("Exception while saving user data: " + e);
        }
    }

    public async void LogIn(string email, string password, Action<string> onLoginSuccess, Action<string> onLoginFail)
    {
        try
        {
            FirebaseUser newUser = (await auth.SignInWithEmailAndPasswordAsync(email, password)).User;
            Debug.Log("User logged in successfully: " + newUser.Email);
            await CheckAndLoadUserData(newUser);
            onLoginSuccess?.Invoke(newUser.Email);
        }
        catch (Exception e)
        {
            Debug.LogError("Login error: " + e.Message);
            onLoginFail?.Invoke(e.Message);
        }
    }

    private async Task CheckAndLoadUserData(FirebaseUser newUser)
    {
        DocumentReference docRef = db.Collection("users").Document(newUser.UserId);

        try
        {
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                User userData = snapshot.ConvertTo<User>();
                userData.userID = newUser.UserId;
                Debug.Log("User data loaded successfully: " + userData.email);
                GameManager.Instance.SetUserData(userData);
            }
            else
            {
                Debug.LogError("User data not found.");
            }
            LoadMainScene();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading user data: " + ex);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ResetPassword(string email, Action<string> onSuccess, Action<string> onFailure)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Password reset was canceled.");
                onFailure?.Invoke("Password reset was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Password reset encountered an error: " + task.Exception);
                onFailure?.Invoke("Error: " + task.Exception.Message);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            onSuccess?.Invoke("Password reset email sent successfully.");
        });
    }
}
