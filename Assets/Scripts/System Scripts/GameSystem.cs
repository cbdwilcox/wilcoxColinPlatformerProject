using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public TMP_Text MoveKeys;

    //bool Paused = false;

    public int CurrentScene;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveKeys.enabled = false;
        }

        //========================================
        // DEBUG CONTROLS
        //========================================

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.Escape) && !Paused)
        //{
        //    Debug.Log("Game Paused");
        //    Time.timeScale = 0;
        //    Paused = true;
        //    Invoke("PauseSet", 0.5f);
        //}

        //if (Input.GetKeyDown(KeyCode.Escape) && Paused)
        //{
        //    Debug.Log("Game Unpaused");
        //    Time.timeScale = 1;
        //    Paused = false;
        //    Invoke("UnpauseSet", 0.5f);
        //}
    }

    //public void PauseSet()
    //{
    //    Paused = true;
    //}

    //public void UnpauseSet()
    //{
    //    Paused = false;
    //}


    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(CurrentScene);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(7);
    }
    public void QuitDesktop()
    {
        Application.Quit();
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadWorkshop()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadAst1()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadAst2()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadAst3()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadVegaFight()
    {
        SceneManager.LoadScene(5);
    }
}
