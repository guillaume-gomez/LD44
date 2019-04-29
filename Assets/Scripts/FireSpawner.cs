using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float spawnMin = 2.0f;
    public float spawnMax = 3.0f;

    public GameObject[] victimsTiles;

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
      if(house != null && !house.HasFire())
      {
        house.Fire();

        GameObject bubble = house.GetBubble();
        GameObject toInstantiate = victimsTiles [Random.Range (0, victimsTiles.Length)];

        TextMesh moneyText = bubble.transform.GetChild(0).GetComponent<TextMesh>();
        moneyText.text = toInstantiate.GetComponent<Victim>().price + "$";

        //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
        GameObject instance =
          Instantiate (toInstantiate, new Vector3 (
                                        bubble.transform.position.x,
                                        bubble.transform.position.y,
                                        -1f), Quaternion.identity
                                      ) as GameObject;

        //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
        instance.transform.SetParent (bubble.gameObject.transform);
      }
      //next call
      Invoke ("SpawnFire", Random.Range (spawnMin, spawnMax));
    }
}
