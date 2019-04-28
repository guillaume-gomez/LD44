using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayMeshText : MonoBehaviour
{
    private Renderer textRenderer;
    void Awake()
    {
      textRenderer = gameObject.GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        textRenderer.sortingLayerName = "InGameUi";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
