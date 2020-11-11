using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] GenerateStageAndBackground generateStageAndBackground;
    float destroyBackgroundPointX;
    GameObject stagePatternGenerator;
    // Start is called before the first frame update
    void Start()
    {
        destroyBackgroundPointX = -60f;
        stagePatternGenerator = GameObject.Find("StagePatternGenerator");
        generateStageAndBackground = stagePatternGenerator.GetComponent<GenerateStageAndBackground>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = generateStageAndBackground.stagePatternMoveSpeed * generateStageAndBackground.speedRate * Time.deltaTime;
        transform.Translate(moveX/4f, 0, 0);
        if (transform.position.x < destroyBackgroundPointX)
        {
            Destroy(this.gameObject);
        }
    }
}
