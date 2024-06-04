using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroList", menuName = "ScriptableObjects/HeroList", order = 1)]
public class HeroList : ScriptableObject
{
    public List<Hero> heroes;

    private void OnEnable()
    {
        if (heroes == null || heroes.Count == 0)
        {
            heroes = new List<Hero>
            {
                new Hero() // 기본 생성자를 사용하여 Hero 인스턴스 생성
            };
        }
    }
}
