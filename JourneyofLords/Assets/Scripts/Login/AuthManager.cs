using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    private FirebaseFirestore db;
    private FirebaseAuth auth;

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
        try
        {
            User newUserEntry = new User
            {
                email = newUser.Email,
                IGN = "u" + DateTime.Now.ToString("yyMMddHHmmssFFFFFFF"),
                gold = 10,
                userLV = 1,
                userEXP = 0,
                wood = 10000,
                stone = 10000,
                iron = 10000,
                food = 10000,
                max_Stage = 0,
                max_Rewards = 0,
                maxHeroes = 12,
                money = 2000,
                troops = 1000,
                userID = newUser.UserId,
                energy = 11,
                maxEnergy = 11
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
                FirestoreManager firestoreManager = FindAnyObjectByType<FirestoreManager>();
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

    public async void ResetPassword(string email, Action<string> onSuccess, Action<string> onFailure)
    {
        // Firestore에서 이메일 확인
        CollectionReference usersRef = db.Collection("users");
        Query query = usersRef.WhereEqualTo("email", email);
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        if (querySnapshot.Documents.Any()) // LINQ를 사용하여 Count 대신 Any를 사용합니다.
        {
            // 이메일이 존재하면 비밀번호 재설정 이메일 발송
            await auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Password reset was canceled.");
                    onFailure?.Invoke("비밀번호 찾기 취소.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Password reset encountered an error: " + task.Exception);
                    onFailure?.Invoke("에러: " + task.Exception.Message);
                    return;
                }

                Debug.Log("Password reset email sent successfully.");
                onSuccess?.Invoke("입력한 메일 주소로 메일이 발송되었습니다.");
            });
        }
        else
        {
            // 이메일이 존재하지 않으면 실패 메시지 반환
            Debug.LogError("Email not found in Firestore.");
            onFailure?.Invoke("등록 되지 않은 이메일 입니다.");
        }
    }
}
