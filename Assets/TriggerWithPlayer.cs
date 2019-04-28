using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithPlayer : MonoBehaviour
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
      if(building.HasFire() && other.gameObject.tag == "Player")
      {
        building.PutOutFire();
        other.gameObject.GetComponent<Player>().PutOutFire();
      }
    }
}
