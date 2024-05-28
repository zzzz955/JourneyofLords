using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject emptyPrefab; // 빈 객체 프리팹
    public Transform enemyGridParent; // GridLayoutGroup을 가진 부모 오브젝트

    private int gridSize = 9; // 3x3 그리드 크기

    public void PlaceEnemies(List<Enemy> enemyList)
    {
        // 모든 그리드 셀 초기화 (기존 오브젝트 제거)
        foreach (Transform child in enemyGridParent)
        {
            Destroy(child.gameObject);
        }

        // 적군 위치를 저장할 Dictionary를 생성합니다.
        Dictionary<int, Enemy> enemyPositions = new Dictionary<int, Enemy>();

        foreach (var enemy in enemyList)
        {
            int index = ConvertToGridIndex(enemy.position);
            Debug.Log($"Converted position {enemy.position} to index {index}");
            if (index >= 0 && index < gridSize)
            {
                // 적군 위치를 Dictionary에 추가합니다.
                enemyPositions[index] = enemy;
            }
            else
            {
                Debug.LogError($"Invalid grid position: {enemy.position} converted to index {index}");
            }
        }

        // 3x3 그리드 크기에 맞게 모든 셀을 처리합니다.
        for (int i = 0; i < gridSize; i++)
        {
            Transform cell = enemyGridParent;

            if (enemyPositions.ContainsKey(i))
            {
                // 적군이 할당된 경우 적 오브젝트 생성
                GameObject enemyObject = Instantiate(enemyPrefab, cell);
                var enemyScript = enemyObject.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    enemyScript.Initialize(enemyPositions[i].hero);
                }
                Debug.Log($"Placed enemy at index {i}");
            }
            else
            {
                // 적군이 할당되지 않은 경우 빈 오브젝트 생성
                Instantiate(emptyPrefab, cell);
                Debug.Log($"Placed empty object at index {i}");
            }
        }
    }

    private int ConvertToGridIndex(Vector2Int gridPosition)
    {
        Debug.Log($"Converting grid position {gridPosition}");
        // 3x3 그리드의 인덱스를 계산합니다.
        return gridPosition.y * 3 + gridPosition.x;
    }
}
