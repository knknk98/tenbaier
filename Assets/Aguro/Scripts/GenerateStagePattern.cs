using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStagePattern : MonoBehaviour
{
    [SerializeField] List<GameObject> stagePatternObjectList = new List<GameObject>();
    [SerializeField] float generateStagePointX;

    //一番右にあるステージオブジェクトを保存するため
    //Inspector上で右端のステージオブジェクトを代入する必要があります。
    [SerializeField] GameObject rightSideStagePatternObject;

    //ステージが時間につれて速くなっていく加速度
    [SerializeField] float stageSpeedAcceleration;

    //スピードはマイナスの値なので、最大マイナス値を制限
    [SerializeField] float stageSpeedMax;

    [SerializeField] float initialStageSpeed;

    [SerializeField] bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        MoveStagePattern.stagePatternMoveSpeed = initialStageSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver==true)
        {
            return;
        }
        if (MoveStagePattern.stagePatternMoveSpeed>=stageSpeedMax)
        {
        MoveStagePattern.stagePatternMoveSpeed += stageSpeedAcceleration;
        }

        if (rightSideStagePatternObject.transform.position.x<generateStagePointX)
        {
            rightSideStagePatternObject=Instantiate(stagePatternObjectList[Random.Range(0, 2)], this.transform.position, Quaternion.identity);
        }
    }

    public void GameOver()
    {
        MoveStagePattern.stagePatternMoveSpeed = 0.0f;
        isGameOver = true;
    }
}