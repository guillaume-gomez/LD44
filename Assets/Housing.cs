using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
    public GameObject fireObject;
    // Start is called before the first frame update
    void Start()
    {
      fireObject.SetActive(false);
      GameManager.instance.AddHousingToList(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Fire()
    {
      fireObject.SetActive(true);
    }
}
