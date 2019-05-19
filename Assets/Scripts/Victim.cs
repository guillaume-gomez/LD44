using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public int price = 0;
    public int age = 0;
    public float distance = 0;
    public float karma = 0;
    public float duration = 0;
    public string soundName;

    private float startTimer;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
        PlayHelp();
    }

    // Update is called once per frame
    void Update()
    {
        float timerGameInSecond = startTimer + (duration);
        currentTime = duration - (Time.time - startTimer);
    }

    public float GetCurrentTimer()
    {
        return currentTime;
    }

    public string GetCurrentTimerString()
    {
        string minutes = (((int) currentTime) / 60).ToString();
        string seconds = (currentTime % 60).ToString("f2");

        return minutes + ":" + seconds;
    }

    public void StartTimer()
    {
        startTimer = Time.time;
    }

    private void PlayHelp()
    {
        AudioManager.instance.PlaySound(soundName, gameObject.transform.position);
    }
}
