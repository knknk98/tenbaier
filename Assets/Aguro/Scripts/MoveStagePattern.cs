using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStagePattern : MonoBehaviour
{
    //マイナスの値で左に移動
    //staticにしているので、これを変更すればステージ全体の移動速度が変わります
    //GenerateStagePattern.csでステージ全体の移動速度を調整しています
    public static float stagePatternMoveSpeed;
    [SerializeField] float destroyStagePointX;

    // Start is called before the first frame update
    void Start()
    {
        destroyStagePointX = -36f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(stagePatternMoveSpeed, 0, 0);
        if (transform.position.x < destroyStagePointX)
        {
            Destroy(this.gameObject);
        }
    }
}
