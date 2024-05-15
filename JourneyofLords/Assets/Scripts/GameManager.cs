using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 변수
    private static GameManager instance;

    // 외부에서 접근할 수 있는 싱글톤 인스턴스
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에만 새로운 인스턴스를 생성합니다.
            if (instance == null)
            {
                // Scene에 GameManager 오브젝트가 있는지 확인합니다.
                instance = FindObjectOfType<GameManager>();

                // 만약 Scene에 GameManager 오브젝트가 없는 경우에는 새로 생성합니다.
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    // 게임 초기화 함수
    public void InitializeGame()
    {
        // 게임 초기화 작업을 수행합니다.
    }
    
    // 게임 종료 함수
    public void QuitGame()
    {
        // 게임 종료 작업을 수행합니다.
        Application.Quit();
    }

    // 다른 함수들을 추가할 수 있습니다.
}
