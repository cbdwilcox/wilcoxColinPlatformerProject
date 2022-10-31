using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public TMP_Text MoveKeys;

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
