using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
    public RectTransform content;
    public float viewHeight;

    CustomScrollView<Item, BagItem> customSV;


    private void Start()
    {
        customSV = new CustomScrollView<Item, BagItem>();
        customSV.Init("UI/Item", content, viewHeight, 3, BagMgr.GetInstance().itemList, 180, 180, 20, 20);
    }

    public override void ShowSelf()
    {
        base.ShowSelf();
    }

    private void Update()
    {
        customSV.CheckShowOrHide();
    }

}
