using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<int, int> itemCountDictionary;
    
    private static ScoreManager singletonInstance;

    public static ScoreManager SinngletonInstance
    {
        get
        {
            if (singletonInstance == null)
            {
                var obj = new GameObject("Scoremanager");
                DontDestroyOnLoad(obj);
                singletonInstance = obj.AddComponent<ScoreManager>();
            }

            return singletonInstance;
        }
    }

    public void AddItem(int itemID, int itemCount)
    {
        if (!itemCountDictionary.ContainsKey(itemID))
        {
            itemCountDictionary[itemID] = 0;
        }

        itemCountDictionary[itemID] += itemCount;
    }

    public Dictionary<int, int> GetItemCountDictionary()
    {
        return itemCountDictionary;
    }
}
