using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject pausePanel;
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & PlayerHealth.isAlive)
        {
            if (!isPaused)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
            else if (isPaused)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;
            }

        }
    }
}
