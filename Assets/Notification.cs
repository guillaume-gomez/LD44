using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private float distance = 20.0f;
    private GameObject cameraRef;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
      cameraRef =  GameObject.FindWithTag("MainCamera");
      // find the parent target
    }

    // Update is called once per frame
    void Update()
    {
      Vector2 cameraPosition = cameraRef.transform.position;
      Debug.Log(cameraPosition);
      if(Vector2.Distance(target.position, cameraPosition) < distance) {
        gameObject.SetActive(true);
      } else {
        gameObject.SetActive(false);
      }
    }
}
