using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButtonScript : MonoBehaviour
{
    public void GameStart(){
        // TODO: 遷移先を本番用シーンに変える
        SceneManager.LoadScene("SampleScene");
    }
}
