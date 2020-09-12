using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStagePattern : MonoBehaviour
{
    [SerializeField] List<GameObject> stagePatternObjectList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate( 生成するオブジェクト,  場所, 回転 );  回転はそのままなら↓
            //Instantiate(target, new Vector3(1.0f, 2.0f, 0.0f), Quaternion.identity);
        }
    }
}
