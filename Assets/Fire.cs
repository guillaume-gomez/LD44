using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Housing building;
    // Start is called before the first frame update
    void Start()
    {
        building = transform.parent.gameObject.GetComponent<Housing>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      Debug.Log(other.gameObject.tag);
      if(other.gameObject.tag == "Water")
      {
        building.PutOutFire();
      }
    }
}