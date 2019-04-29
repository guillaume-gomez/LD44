using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
    public GameObject fireObject;
    public GameObject bubbleObject;

    private Color fireColor;
    private TextMesh timerText;

    public AudioClip fireStartAudio;
    public AudioClip fireEndAudio;
    // Start is called before the first frame update
    void Start()
    {
      fireObject.SetActive(false);
      fireColor = fireObject.GetComponent<Renderer> ().material.color;
      bubbleObject.SetActive(false);
      timerText = bubbleObject.transform.GetChild(1).GetComponent<TextMesh>();

      GameManager.instance.AddHousingToList(this);
    }

    // Update is called once per frame
    void Update()
    {
      // we assume that victim is the third items
      if(bubbleObject.transform.childCount > 2)
      {
        Victim victim = bubbleObject.transform.GetChild(2).GetComponent<Victim>();
        if(victim)
        {
          timerText.text = victim.GetCurrentTimerString();
          if(victim.GetCurrentTimer() < 0.0f)
          {
             DestroyVictimAndHud();
             bubbleObject.SetActive(false);
          }
        }
      }

    }

    public GameObject GetBubble()
    {
      return bubbleObject;
    }

    public bool HasFire()
    {
      return fireObject.activeSelf && fireColor.a == 1.0f;
    }

    public void Fire()
    {
      fireObject.SetActive(true);
      bubbleObject.SetActive(true);
      SoundManager.instance.PlayOnLoop(fireStartAudio);

      Color color = fireObject.GetComponent<Renderer> ().material.color;
      color.a = 1.0f;
      GetComponent<Renderer> ().material.color = color;
    }

    public void PutOutFire()
    {
      StartCoroutine(FadeFireAlpha(1f,0f, fireObject));
      //assume that Coroutine is over after 1.5sec (see FadeFireAlpha)
      Invoke("AddMoney", 1.5f);
    }

    private void AddMoney()
    {
      //texts are inserted in the bubbleObjectHierarchy, so Victim is the third item
      if(bubbleObject.transform.childCount > 2)
      {
        Victim currentVictim = bubbleObject.transform.GetChild(2).GetComponent<Victim>();
        if(currentVictim)
        {
          GameManager.instance.AddMoney(currentVictim.price);
        }
      }
    }

    private void DestroyVictimAndHud()
    {
      foreach (Transform child in bubbleObject.transform)
      {
        Destroy(child.gameObject);
      }
    }

    private IEnumerator FadeFireAlpha(float startAlpha, float endAlpha, GameObject fireObject)
    {

      float elapsedTime = 0f;
      float totalDuration = 1.5f;

      while (elapsedTime < totalDuration)
      {
          elapsedTime += Time.deltaTime;
          float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);
          fireColor.a = currentAlpha;
          fireObject.GetComponent<Renderer> ().material.color = fireColor;
          yield return null;
      }
      SoundManager.instance.PlaySingle(fireEndAudio);
      bubbleObject.SetActive(false);
    }
}
