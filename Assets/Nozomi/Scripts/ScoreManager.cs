using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public struct ItemInfo
    {
        public Sprite thumbnail;
        public string name;
        public int price;
        public int count;
    }
    private Dictionary<int, ItemInfo> itemDictionary = new Dictionary<int, ItemInfo>();
    
    private static ScoreManager singletonInstance;

    public static ScoreManager SingletonInstance
    {
        get
        {
            if (singletonInstance == null)
            {
                var obj = new GameObject("ScoreManager");
                DontDestroyOnLoad(obj);
                singletonInstance = obj.AddComponent<ScoreManager>();
            }

            return singletonInstance;
        }
    }

    public void AddItem(int itemID, int itemCount)
    {
        if (!itemDictionary.ContainsKey(itemID))
        {
            var itemData = ItemScriptalbleObject.SingletonInstance.ItemDataList[itemID];
            itemDictionary[itemID] = new ItemInfo(){thumbnail = itemData.thumbnail, name = itemData.name, price = itemData.price, count = 0};
        }

        var item = itemDictionary[itemID];
        item.count += itemCount;
        itemDictionary[itemID] = item;
    }

    public List<ItemInfo> GetItemList()
    {
        var itemList = itemDictionary.Values.OrderBy(v => v.price).ToList();
        return itemList;
    }
    
}
