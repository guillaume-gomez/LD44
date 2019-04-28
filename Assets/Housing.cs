using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
    public GameObject fireObject;
    public GameObject bubbleObject;

    private Color fireColor;
    // Start is called before the first frame update
    void Start()
    {
      fireObject.SetActive(false);
      fireColor = fireObject.GetComponent<Renderer> ().material.color;
      bubbleObject.SetActive(false);
      GameManager.instance.AddHousingToList(this);
    }

    // Update is called once per frame
    void Update()
    {
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
      Color color = fireObject.GetComponent<Renderer> ().material.color;
      color.a = 1.0f;
      GetComponent<Renderer> ().material.color = color;
    }

    public void PutOutFire()
    {
      StartCoroutine(FadeFireAlpha(1f,0f, fireObject));
      Victim currentVictim = bubbleObject.transform.GetChild(0).GetComponent<Victim>();
      Debug.Log(currentVictim.price);
      GameManager.instance.AddMoney(currentVictim.price);
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
      bubbleObject.SetActive(false);
    }
}
