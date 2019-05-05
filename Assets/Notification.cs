using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private float distance = 20.0f;
    private float distanceToDisplay = 40.0f;
    private GameObject cameraRef;
    private Transform target;
    private float cameraWidth;
    private float cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
      cameraRef = GameObject.FindWithTag("MainCamera");
      Camera camera = cameraRef.GetComponent<Camera>();
      // find the parent target
      cameraHeight = 2.0f * camera.orthographicSize;
      cameraWidth = cameraHeight * camera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
      Vector2 cameraPosition = cameraRef.transform.position;
      Vector2 parentPosition = transform.parent.transform.position;
      Vector2 hypothenuse = new Vector2(parentPosition.x - cameraPosition.x, parentPosition.y - cameraPosition.y);
      hypothenuse.Normalize();

      Vector2 newPosition = hypothenuse * distanceToDisplay;
      //Debug.Log(newPosition);
      //Debug.Log(transform.position);
      Debug.Log(cameraPosition);
      transform.position = cameraPosition + hypothenuse;
      /*if(Vector2.Distance(parentPosition, cameraPosition) < distance) {
        gameObject.SetActive(false);
      } else {
        gameObject.SetActive(true);
      }*/
    }
}
