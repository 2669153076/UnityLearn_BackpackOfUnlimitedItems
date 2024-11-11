using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于初始化格子方法
/// </summary>
/// <typeparam name="T"></typeparam>
public interface I_ItemBase<T>
{
    public  void InitItemInfo(T item);
}

/// <summary>
/// 自定义ScrollView
/// 动态生成视野内道具
/// </summary>
/// <typeparam name="T">数据来源</typeparam>
/// <typeparam name="K">格子UI</typeparam>
public class CustomScrollView<T,K> where  K : I_ItemBase<T>
{
    public RectTransform content;   //通过Content得到可视范围，并将道具设置为子物体

    public float viewPortH;   //可视范围的高
    public int col; //列数
    public float spacingX = 0;    //间隔X距离
    public float spacingY = 0;    //间隔Y距离
    public float itemWidth = 0; //格子宽
    public float itemHeight = 0;    //格子高
    private string itemResPath; //格子路径



    private Dictionary<int, GameObject> curShowItemDic = new Dictionary<int, GameObject>();  //当前显示着的格子对象

    private List<T> itemList;    //数据来源

    private int beforeMinIndex = -1;    //上一次显示的最小索引的格子
    private int beforeMaxIndex = -1;    //上一次显示的最大索引的格子


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="content"></param>
    /// <param name="viewPortH">可视高度</param>
    /// <param name="col">列数</param>
    /// <param name="itemList">Item列表</param>
    /// <param name="itemWidth">道具的宽</param>
    /// <param name="itemHeight">道具的高</param>
    /// <param name="spaceX">间隔X</param>
    /// <param name="spaceY">间隔Y</param>
    public void Init(string itemResPath,RectTransform content, float viewPortH, int col, List<T> itemList, float itemWidth, float itemHeight, float spaceX = 0, float spaceY = 0)
    {
        this.itemResPath = itemResPath;
        this.content = content;
        this.viewPortH = viewPortH;
        this.col = col;
        this.itemList = itemList;
        this.itemWidth = itemWidth;
        this.itemHeight = itemHeight;
        this.spacingX = spaceX;
        this.spacingY = spaceY;
        this.content.sizeDelta = new Vector2(0, Mathf.CeilToInt(itemList.Count / (float)col) * (itemHeight + spacingY));
    }

    /// <summary>
    /// 检测哪些格子应该显示
    /// </summary>
    public void CheckShowOrHide()
    {
        int minIndex = (int)(content.anchoredPosition.y / (itemHeight+spacingY)) * col; //可视范围的起始的y/一个格子的高=》起始显示的是哪一行*一行的格子数=起始位置显示的索引值
        int maxIndex = (int)((content.anchoredPosition.y + viewPortH) / (itemHeight + spacingY)) * col + (col-1); //（可视范围的起始的y+可视范围的高）=》可视范围结束y/一个格子的高*一行格子数=》结束位置时是哪一行+（一行格子数-1）=结束位置显示的索引值
        if (minIndex <= 0)
            minIndex = 0;
        if (maxIndex >= itemList.Count)
            maxIndex = itemList.Count - 1;

        if (minIndex != beforeMinIndex || maxIndex != beforeMaxIndex)
        {
            for (int i = beforeMinIndex; i < minIndex; i++)
            {
                if (curShowItemDic.ContainsKey(i) && curShowItemDic[i] != null)
                    PoolMgr.GetInstance().Push(GetObjName(itemResPath), curShowItemDic[i]);
                curShowItemDic.Remove(i);
            }
            for (int i = maxIndex + 1; i <= beforeMaxIndex; i++)
            {
                if (curShowItemDic.ContainsKey(i) && curShowItemDic[i] != null)
                    PoolMgr.GetInstance().Push(GetObjName(itemResPath), curShowItemDic[i]);
                curShowItemDic.Remove(i);

            }
            beforeMinIndex = minIndex;
            beforeMaxIndex = maxIndex;
        }


        //创建指定索引范围内的格子
        for (int i = minIndex; i <= maxIndex; i++)
        {
            if (curShowItemDic.ContainsKey(i))
            {
                continue;
            }
            else
            {
                int index = i;
                curShowItemDic.Add(index, null);
                PoolMgr.GetInstance().Get(itemResPath, (item) =>
                {
                    item.transform.SetParent(content);      //设置父物体
                    item.transform.localScale = Vector3.one;    //设置缩放
                    item.transform.localPosition = new Vector3((index % col) * (itemWidth + spacingX), -index / col * (itemHeight+spacingY), 0); //设置相对位置
                    item.GetComponent<K>().InitItemInfo(itemList[index]);    //初始化

                    if (curShowItemDic.ContainsKey(index))
                    {
                        curShowItemDic[index] = item;
                    }
                    else
                    {
                        PoolMgr.GetInstance().Push(GetObjName(itemResPath), item);
                    }
                });
            }

        }

        
    }

    /// <summary>
    /// 获取物体名称
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string GetObjName(string path)
    {
        int lastIndex = path.LastIndexOf("/");
        return path.Substring(lastIndex + 1);
    }
}