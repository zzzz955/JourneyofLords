using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPopup : MonoBehaviour
{
    public GameObject equipdic;
    public void IsClickHero(string type) {
        
    }

    public void ShowEquipDic() {
        GameObject dic = Instantiate(equipdic, transform.parent);
        EquipDic equipDic = dic.GetComponent<EquipDic>();
    }
}
