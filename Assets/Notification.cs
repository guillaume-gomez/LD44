using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private GameObject cameraRef;
    private Renderer parentRenderer;
    private float cameraWidth;
    private float cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
      cameraRef = GameObject.FindWithTag("MainCamera");
      parentRenderer = transform.parent.GetComponent<Renderer>();

      Camera camera = cameraRef.GetComponent<Camera>();
      cameraHeight = 2.0f * camera.orthographicSize;
      cameraWidth = cameraHeight * camera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
      if(parentRenderer.isVisible) {
        gameObject.SetActive(false);
      } else {
        Vector2 cameraPosition = cameraRef.transform.position;
        Vector2 parentPosition = transform.parent.transform.position;
        Vector2 hypothenuse = new Vector2(parentPosition.x - cameraPosition.x, parentPosition.y - cameraPosition.y);
        hypothenuse.Normalize();

        Vector2 originHeight = new Vector2(0.0f, cameraHeight/2.0f);
        Vector2 originWidth = new Vector2(cameraWidth/2.0f, 0.0f);

        Vector2 newPosition = new Vector2(Vector2.Dot(originWidth, hypothenuse), Vector2.Dot(originHeight, hypothenuse));
        transform.position = cameraPosition + newPosition;
        gameObject.SetActive(true);
      }
    }
}
