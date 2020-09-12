using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    float destroyBackgroundPointX;
    // Start is called before the first frame update
    void Start()
    {
        destroyBackgroundPointX = -60f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(GenerateStageAndBackground.stagePatternMoveSpeed/4f, 0, 0);
        if (transform.position.x < destroyBackgroundPointX)
        {
            Destroy(this.gameObject);
        }
    }
}
