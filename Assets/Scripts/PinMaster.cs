using UnityEngine;
using System.Collections.Generic;

public class PinMaster : MonoBehaviour {

     private Pin[] pins;
     private List<Pin> fallen = new List<Pin>();

     public List<Pin> GetFallenPins() {
          fallen.Clear();

          foreach (Pin p in pins) {
               if (!p.IsStanding() && p.gameObject.activeInHierarchy)
                    fallen.Add(p);
          }

          return fallen;
     }

     public void RaiseAll() {
          foreach (Pin p in pins)
               p.Raise();
     }

     public void LowerAll() {
          foreach (Pin p in pins)
               p.Lower();
     }

     public void CleanAllPins() {
          foreach (Pin p in pins) {
               p.gameObject.SetActive(true);
          }
     }

     public void CleanAllFallen() {
          foreach (Pin f in fallen) {
               f.gameObject.SetActive(false);
          }
     }

     public bool AllPinsStill() {
          return false;
     }

	// Use this for initialization
	void Start () {
          pins = GameObject.FindObjectsOfType<Pin>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
