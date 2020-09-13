using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.SingletonInstance.PlayBGM("titleBGM", true, 50.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
