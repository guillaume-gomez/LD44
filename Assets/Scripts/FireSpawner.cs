using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float spawnMin = 2.0f;
    public float spawnMax = 3.0f;

    public GameObject[] victims;

    void Start()
    {
    }

    void Update()
    {

    }

    public void StartFires()
    {
      SpawnFire();
    }

    private void SpawnFire() {
      Housing house = GameManager.instance.GetRandomHousing();
      if(house != null)
      {
        Debug.Log("Fire");
        house.Fire();
      }
      //next call
      Invoke ("SpawnFire", Random.Range (spawnMin, spawnMax));
    }
}
