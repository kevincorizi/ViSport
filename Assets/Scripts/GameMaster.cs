using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
          foreach (Pin p in GameObject.FindObjectsOfType<Pin>()) {
               bool fell = p.Fell;
               if (!fell && !p.IsStanding())
                    Debug.Log("Pin fell!");
          }
	}
}
