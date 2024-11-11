using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BagMgr.GetInstance().InitInfo();
        UIManager.GetInstance().ShowPanel<BasePanel>("BagPanel",E_UI_Layer.Bot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
