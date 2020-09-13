using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string DestinationScene;
    public void Transition(){
        SceneManager.LoadScene(DestinationScene);
    }
}
