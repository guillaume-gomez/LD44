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
    //public AudioClip VictimDiedAudio;
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
      Victim victim = GetVictim();
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

    public GameObject GetBubble()
    {
      return bubbleObject;
    }

    public Victim GetVictim()
    {
      if(bubbleObject.transform.childCount <= 2) {
        return null;
      }
      return bubbleObject.transform.GetChild(2).GetComponent<Victim>();
    }

    public bool HasFire()
    {
      return fireObject.activeSelf && fireColor.a > 0.0f;
    }

    public void Fire()
    {
      fireObject.SetActive(true);
      bubbleObject.SetActive(true);
      SoundManager.instance.PlayOnLoop(fireStartAudio);
    }

    public void PutOutFire()
    {
      StartCoroutine(FadeFireAlpha(1f,0f, fireObject));
      //assume that Coroutine is over after 1.5sec (see FadeFireAlpha)
      Invoke("UpdateScore", 1.5f);
    }

    private void UpdateScore()
    {
      //texts are inserted in the bubbleObjectHierarchy, so Victim is the third item
      if(bubbleObject.transform.childCount > 2)
      {
        Victim currentVictim = bubbleObject.transform.GetChild(2).GetComponent<Victim>();
        if(currentVictim)
        {
          GameManager.instance.AddMoney(currentVictim.price);
          GameManager.instance.EditKarma(currentVictim.karma);
        }
      }
    }

    private void DestroyVictimAndHud()
    {
      //destroy the Victim
      Destroy(bubbleObject.transform.GetChild(2).gameObject);
      GameManager.instance.EditKarma(-GameManager.noSaveVictim);

      fireObject.SetActive(false);
      Color fireColorBase = new Color(1.0f, 1.0f, 1.0f, 0.0f);
      fireColor = fireColorBase;
      fireObject.GetComponent<Renderer> ().material.color = fireColorBase;

      //SoundManager.instance.PlaySingle(VictimDiedAudio);
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
      fireObject.SetActive(false);

      Color fireColorBase = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      fireColor = fireColorBase;
      fireObject.GetComponent<Renderer> ().material.color = fireColorBase;
    }
}
