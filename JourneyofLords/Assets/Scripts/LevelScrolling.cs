using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScrolling : MonoBehaviour
{
    public GameObject levelButtonPrefab; // 레벨 버튼 프리팹
    public int numberOfLevels; // 생성할 레벨 버튼의 수
    public GridLayoutGroup gridLayoutGroup; // Content의 GridLayoutGroup

    void Start()
    {
        // gridLayoutGroup 변수가 올바르게 할당되었는지 확인
        if(gridLayoutGroup == null)
        {
            Debug.LogError("gridLayoutGroup이 할당되지 않았습니다.");
            return;
        }

        // levelButtonPrefab이 올바르게 설정되었는지 확인
        if(levelButtonPrefab == null)
        {
            Debug.LogError("levelButtonPrefab이 설정되지 않았습니다.");
            return;
        }

        // 필요한 만큼 레벨 버튼 생성
        for (int i = 0; i < numberOfLevels; i++)
        {
            // 레벨 버튼 생성
            GameObject levelButtonGO = Instantiate(levelButtonPrefab, gridLayoutGroup.transform);
            Button levelButton = levelButtonGO.GetComponent<Button>();

            // levelButton이 올바른지 확인
            if (levelButton == null)
            {
                Debug.LogError("레벨 버튼이 올바르게 가져와지지 않았습니다.");
                return;
            }

            // // 버튼 텍스트 설정
            // Text buttonText = levelButton.GetComponentInChildren<Text>();
            // if(buttonText != null)
            // {
            //     buttonText.text = "Level " + (i + 1);
            // }
            // else
            // {
            //     Debug.LogError("버튼 프리팹에 Text 컴포넌트가 없습니다.");
            // }
        }

    }
}