using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemScriptalbleObject : ScriptableObject
{
    public const string PATH = "ItemScriptableObject";
    //MyScriptableObjectの実体
    private static ItemScriptalbleObject singletonInstance;
    public  static ItemScriptalbleObject  SingletonInstance{
        get{
            //初アクセス時にロードする
            if(singletonInstance == null){
                singletonInstance = Resources.Load<ItemScriptalbleObject>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if(singletonInstance == null){
                    Debug.LogError(PATH + " not found");
                }
            }

            return singletonInstance;
        }
    }
    
    public List<ItemData> ItemDataList = new List<ItemData>();
}

[System.Serializable]
public struct ItemData
{
    public Sprite thumbnail;
    public string name;
    public int price;
}
