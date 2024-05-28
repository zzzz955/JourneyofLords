using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemyGridParent;

    public void PlaceEnemies(List<Enemy> enemyList)
    {
        foreach (var enemy in enemyList)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, enemyGridParent);
            enemyObject.GetComponent<RectTransform>().anchoredPosition = GetGridPosition(enemy.position);
            Debug.Log($"Placing enemy at grid position: {enemy.position}");

            var enemyScript = enemyObject.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(enemy.hero);
            }
        }
    }

    private Vector2 GetGridPosition(Vector2Int gridPosition)
    {
        float cellSize = 100f; // Adjust based on your GridLayoutGroup settings
        float x = gridPosition.x * cellSize;
        float y = -gridPosition.y * cellSize; // Unity UI y-axis is inverted
        return new Vector2(x, y);
    }
}
