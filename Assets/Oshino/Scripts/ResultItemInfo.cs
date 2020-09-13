using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultItemInfo : MonoBehaviour
{
    public Text textL;
    public Text textR;
    public Text priceL;
    public Text priceR;
    // Start is called before the first frame update
    void Start()
    {
            var itemDict = ScoreManager.SingletonInstance.GetItemList();
            int i=0;
            foreach (var item in itemDict)
            {
                if(i<6){
                    textL.text = item.name+"\n";
                    priceL.text = item.price.ToString() +"円×"+item.count.ToString()+"\n";
                }else{
                    textR.text = item.name+"\n";
                    priceR.text = item.price.ToString() +"円×"+item.count.ToString()+"\n";
                }
            }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
