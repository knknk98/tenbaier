using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStagePattern : MonoBehaviour
{

    float destroyStagePointX;

    // Start is called before the first frame update
    void Start()
    {
        destroyStagePointX = -36f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(GenerateStageAndBackground.stagePatternMoveSpeed, 0, 0);
        if (transform.position.x < destroyStagePointX)
        {
            Destroy(this.gameObject);
        }
    }
}
