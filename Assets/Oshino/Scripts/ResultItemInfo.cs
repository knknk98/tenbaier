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
            string textl="";
            string textr="";
            string pricel="";
            string pricer="";
            foreach (var item in itemDict)
            {
                if(i<6){
                    textl += item.name+"\n";
                    pricel += "￥" + item.price.ToString() +"×"+item.count.ToString()+"\n";
                }else{
                    textr += item.name+"\n";
                    pricer += "￥" + item.price.ToString() +"×"+item.count.ToString()+"\n";
                }
            }
            textL.text = textl;
            textR.text = textr;
            priceL.text = pricel;
            priceR.text = pricer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
