using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStagePattern : MonoBehaviour
{
    [SerializeField] GenerateStageAndBackground generateStageAndBackground;
    float destroyStagePointX;
    GameObject stagePatternGenerator;
    // Start is called before the first frame update
    void Start()
    {
        destroyStagePointX = -36f;
        stagePatternGenerator = GameObject.Find("StagePatternGenerator");
        generateStageAndBackground = stagePatternGenerator.GetComponent<GenerateStageAndBackground>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = generateStageAndBackground.stagePatternMoveSpeed * generateStageAndBackground.speedRate;
        transform.Translate(moveX, 0, 0);
        if (transform.position.x < destroyStagePointX)
        {
            Destroy(this.gameObject);
        }
    }
}
