using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public GameObject[] housingsTiles;             //Array of buildings prefabs

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlaceHousing(int x, int y)
    {
      GameObject toInstantiate = housingsTiles [Random.Range (0, housingsTiles.Length)];
      //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
      GameObject instance =
        Instantiate (toInstantiate, new Vector3 (gameObject.transform.position.x + x, gameObject.transform.position.y + y, 0f), Quaternion.identity) as GameObject;

      //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
      instance.transform.SetParent (gameObject.transform);
    }

    public void CreateBuildings()
    {
      PlaceHousing(-4, 1);
      PlaceHousing(0 , 1);
    }
}
