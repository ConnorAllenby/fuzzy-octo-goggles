using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPedAnimations : MonoBehaviour {

	Animation  myAnim;
	public AnimationClip idleAnim;
    // Use this for initialization
    void Start () {
     myAnim = gameObject.GetComponent<Animation>();
	 myAnim.clip = idleAnim;
	 
    }
    
    // Update is called once per frame
    void Update () {
	myAnim.Play();
    }


    
}
