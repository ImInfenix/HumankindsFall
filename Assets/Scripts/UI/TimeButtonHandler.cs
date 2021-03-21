using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButtonHandler : MonoBehaviour
{
    public static TimeButtonHandler instance;

    public GameObject playPause;
    public GameObject speed;

    public int time;
    private int saveTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playPause = Instantiate(playPause, GetComponent<RectTransform>());

        speed = Instantiate(speed, GetComponent<RectTransform>());

        HideTimeButton();

        time = 1;
        saveTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PauseMenu.isGamePaused == false)
        {
            PlayPause();
        }
    }

    public void PlayPause()
    {
        if (time == 1 || time == 2)
        {
            Time.timeScale = 0;
            time = 0;
        }
        else if (time == 0)
        {
            Time.timeScale = saveTime;
            time = saveTime;
        }
    }

    public void HideTimeButton()
    {
        playPause.SetActive(false);
        speed.SetActive(false);
    }

    public void ShowTimeButton()
    {
        playPause.SetActive(true);
        speed.SetActive(true);
    }
}
