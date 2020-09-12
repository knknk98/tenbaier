using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStagePattern : MonoBehaviour
{
    //マイナスの値で左に移動
    //staticにしているので、これを変更すればステージ全体の移動速度が変わると思います
    static float stagePatternMoveSpeed;
    [SerializeField] float destroyStagePointX;

    // Start is called before the first frame update
    void Start()
    {
        stagePatternMoveSpeed = -0.05f;
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
