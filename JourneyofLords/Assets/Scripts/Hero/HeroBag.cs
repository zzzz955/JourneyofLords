// using System.Collections.Generic;
// using UnityEngine;
// using TMPro; // TextMeshPro 네임스페이스 추가
// using UnityEngine.UI;
// using Unity.VisualScripting;

// public class HeroBag : MonoBehaviour
// {
//     public GameObject heroPrefab; // 영웅 정보를 표시할 프리팹
//     public Transform gridTransform; // GridLayoutGroup의 Transform
//     public TMP_Text cntHeroes;

//     private List<Hero> heroes = new List<Hero>();
//     private int displayedHeroes = 0;
//     private int maxHeroes = 20;

//     void Start()
//     {
//     }

//     void Update() {
        
//     }

//     void DisplayHeroes()
//     {
//     }

//     void DisplayMoreHeroes(int amount)
//     {
//         maxHeroes += amount;
//     }

//     void UpdateHeroesCnt() {
//         cntHeroes.SetText($"보유 영웅 {displayedHeroes}/{maxHeroes}");
//     }

//     Sprite LoadSprite(string imagePath)
//     {
//         // 이미지 경로에서 확장자를 제거
//         string resourcePath = imagePath.Replace(".png", "").Replace(".jpg", "");
//         Debug.Log("Attempting to load texture from path: " + resourcePath); // 디버깅을 위한 로그

//         Texture2D texture = Resources.Load<Texture2D>(resourcePath);
//         if (texture == null)
//         {
//             // 만약 png로 시도해서 실패했다면 jpg로 다시 시도
//             texture = Resources.Load<Texture2D>(resourcePath + ".jpg");
//             if (texture == null)
//             {
//                 Debug.LogError("Failed to load texture at path: " + resourcePath);
//                 return null;
//             }
//         }

//         Debug.Log("Successfully loaded texture: " + resourcePath); // 성공적으로 로드된 경우 로그

//         return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//     }
// }
