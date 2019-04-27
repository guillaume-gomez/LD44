using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithPlayer : MonoBehaviour
{
    public GameObject fireObject;
    private Color fireColor;
    // Start is called before the first frame update
    void Start()
    {
      fireColor = fireObject.GetComponent<Renderer> ().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(fireObject.activeSelf && fireColor.a == 1.0f && other.gameObject.tag == "Player")
      {
        StartCoroutine(FadeFireAlpha(1f,0f, fireObject));
        other.gameObject.GetComponent<Player>().PutOutFire();
      }
    }

    public IEnumerator FadeFireAlpha(float startAlpha, float endAlpha, GameObject fireObject)
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
    }
}
