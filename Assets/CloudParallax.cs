using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudParallax : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.0f;

    private List<float> speeds;
    private List<float> directions;
    // Start is called before the first frame update
    void Start()
    {
      speeds = new List<float>();
      directions = new List<float>();
      for(int i = 0; i < gameObject.transform.childCount; i++)
      {
        float speed = Random.Range (minSpeed, maxSpeed);
        float direction = Random.Range(0.0f, 100.0f) > 50.0f ? -1 : 1;
        speeds.Add(speed);
        directions.Add(direction);
      }
    }

    // Update is called once per frame
    void Update()
    {
      int index = 0;
      foreach (Transform child in transform) {
        Vector3 newPosition = new Vector3(
          child.position.x + (speeds[index] * directions[index] *  Time.deltaTime) ,
          child.position.y,
          child.position.z
        );

        if(newPosition.x < -10)
        {
          newPosition.x = 100;
        }
        else if(newPosition.x > 100)
        {
          newPosition.x = -10;
        }


        child.position = newPosition;
        index = index++;
      }
    }
}
