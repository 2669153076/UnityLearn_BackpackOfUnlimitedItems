using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
    public RectTransform content;   //通过Content得到可视范围，并将道具设置为子物体

    public int viewPortH;   //可视范围的高

    /// <summary>
    /// 检测哪些格子应该显示
    /// </summary>
    private void CheckShowOrHide()
    {
        int minIndex = (int)(content.anchoredPosition.y / 200) * 3; //可视范围的起始的y/一个格子的高=》起始显示的是哪一行*一行的格子数=起始位置显示的索引值
        int maxIndex = (int)((content.anchoredPosition.y + viewPortH )/ 200) * 3 + 2; //（可视范围的起始的y+可视范围的高）=》可视范围结束y/一个格子的高=》结束位置时是哪一行+（一行格子数-1）=结束位置显示的索引值

        //创建指定索引范围内的格子
        for (int i = minIndex; i < maxIndex; i++)
        {
            PoolMgr.GetInstance().Get("UI/Item");
        }
    }
}
