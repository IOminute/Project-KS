using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private bool isOKToStart;
    void Start()
    {
        isOKToStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOKToStart && Input.GetKeyDown(KeyCode.Return)) {
            GameStart();
        }
    }

    public void ReadyToStart()
    {
        isOKToStart=true;
    }

    void GameStart()
    {
        SceneManager.LoadScene("Main");
    }
}
