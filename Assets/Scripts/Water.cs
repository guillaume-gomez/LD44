 using UnityEngine;
 using System.Collections;

public class Water : MonoBehaviour {
    public float delay = 0.5f;
    public AudioClip waterSound;

     // Use this for initialization
    void Start () {
      SoundManager.instance.PlaySingle(waterSound);
      Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * delay);
    }
}