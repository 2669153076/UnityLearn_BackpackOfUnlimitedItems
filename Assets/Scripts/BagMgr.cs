using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BagMgr : BaseManager<BagMgr>
{
    public List<Item> itemList = new List<Item>();

    public void InitInfo()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = new Item();
            item.id = i;
            item.num = i;

            itemList.Add(item);
        }
    }
}
