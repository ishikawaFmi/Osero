using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{

    void Update()
    {
        
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Start");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Clear()
    {
        SceneManager.LoadScene("Clear");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
