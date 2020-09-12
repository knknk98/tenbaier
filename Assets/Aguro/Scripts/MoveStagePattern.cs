using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStagePattern : MonoBehaviour
{
    //マイナスの値で左に移動
    [SerializeField] float stagePatternMoveSpeed;
    [SerializeField] float stageDestroyPointX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(stagePatternMoveSpeed, 0, 0);
        if (transform.position.x < stageDestroyPointX)
        {
            Destroy(this.gameObject);
        }
    }
}
