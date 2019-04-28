using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

  public Text timerText;
  private float startTimer;
  public float timerGameInMinutes = 2.0f;
  public bool started = false;

  // Use this for initialization
  void Start () {
  }

  // Update is called once per frame
  void Update () {
    if(started) {
      float timerGameInMilliseconds = startTimer + (timerGameInMinutes * 60);
      float t = timerGameInMilliseconds - (Time.time - startTimer);

      if(t < 0.0f) {
        GameManager.instance.GameOver();
      }

      string minutes = (((int) t) / 60).ToString();
      string seconds = (t % 60).ToString("f2");

      timerText.text = minutes + ":  " + seconds;
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
    startTimer = Time.time;
    started = true;
  }
}
