using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class FirebaseInit : MonoBehaviour
{
    private FirebaseFirestore db;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase is ready.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
            }
        });

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase 초기화 성공");
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {task.Result}");
            }
        });
    }
}
