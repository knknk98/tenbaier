using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStagePattern : MonoBehaviour
{
    [SerializeField] List<GameObject> stagePatternObjectList = new List<GameObject>();
    [SerializeField] float generateStagePointX;

    //一番右にあるステージオブジェクトを保存するために必要
    //Inspector上で右端のステージオブジェクトを代入する必要があります。
    [SerializeField] private GameObject rightSideStagePatternObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (rightSideStagePatternObject.transform.position.x<generateStagePointX)
        {
            //Instantiate( 生成するオブジェクト,  場所, 回転 );  回転はそのままなら↓
            rightSideStagePatternObject=Instantiate(stagePatternObjectList[Random.Range(0, 2)], this.transform.position, Quaternion.identity);
        }
    }
}
