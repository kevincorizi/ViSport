using UnityEngine;
using System.Collections;

public class CheckPinsTrigger : MonoBehaviour {

     private GameObject ball;
     private GameMaster master; 

	// Use this for initialization
	void Start () {
          ball = GameObject.FindObjectOfType<Ball>().gameObject;
          master = GameObject.FindObjectOfType<GameMaster>();
     }

     void OnTriggerEnter(Collider other) {
         if (other.gameObject == ball) {
               master.ballReachedPins = true;
          }
     }
}
