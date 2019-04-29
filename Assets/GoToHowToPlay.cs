using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHowToPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Invoke("Play", 10f);
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown("Submit")) {
        Play();
      }
    }

    void Play()
    {
      SceneManager.LoadScene(2);
    }
}

