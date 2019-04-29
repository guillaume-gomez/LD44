using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

  public Text timerText;
  private float startTimer;
  public float timerGameInMinutes = 2.0f;
  private bool started = false;

  // Use this for initialization
  void Start () {
  }

  // Update is called once per frame
  void Update () {
    if(started) {
      float t = (Time.time - startTimer);
      if(t > (timerGameInMinutes * 60.0f)) {
        GameManager.instance.GameOver();
      }

      float timeRemaining = (timerGameInMinutes * 60.0f) - t;
      string minutes = (((int) timeRemaining) / 60).ToString();
      string seconds = (timeRemaining % 60).ToString("f2");

      timerText.text = minutes + ":" + seconds;
    }
  }

  public void StopTimer() {
    timerText.color = Color.yellow;
    started = false;
  }

  public void SetAsZeroText()
  {
    timerText.text = "0: 00,00";
  }

  public void StartTimer() {
    Debug.Log("StartTimer");
    startTimer = Time.time;
    started = true;
  }
}
