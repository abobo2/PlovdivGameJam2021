using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausemenu : MonoBehaviour
{
    public static bool GameISpaused = false;
    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameISpaused)
            {
                Resume();
            }    else
            {
                Pause();
            }
        }
    }

    void Resume ()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameISpaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameISpaused = true;
    }
}
