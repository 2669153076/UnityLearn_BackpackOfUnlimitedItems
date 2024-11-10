using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : BasePanel
{
    public void InitItemInfo(Item info)
    {
        GetControl<Text>("NumText").text = info.num.ToString();
    }
}
