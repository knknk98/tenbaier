using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    public void GameStart(){
        // TODO: 遷移先を本番用シーンに変える
        SceneManager.LoadScene("SampleScene");
    }
}
