using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string DestinationScene;
    public void Transition(){
        SoundManager.SingletonInstance.PlaySE("click",false,0.3f);
        SceneManager.LoadScene(DestinationScene);
    }
}
