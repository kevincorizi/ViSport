using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LedAnimation : MonoBehaviour {

    public GameObject previousLed = null;
    private float waitingTime = 0.05F;
    public LastLed last_led = null;
    
    

	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {

        if (gameObject.GetComponent<Light>().enabled == false)
        {
            if (previousLed.GetComponent<Light>().enabled == true)
            {
                StartCoroutine(ActivateLed());

            }
        }
        
        //if (last_led.last_is_active == true) {
        //    //StartCoroutine(Blinking());
        //    ////StartCoroutine(Blinking());
        //    gameObject.GetComponent<Light>().enabled = false;
        //}
        
        
	
	}

    IEnumerator ActivateLed() {
        yield return new WaitForSeconds(waitingTime);
        gameObject.GetComponent<Light>().enabled = true;

    }

    IEnumerator Blinking() {
        gameObject.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<Light>().enabled = true;
    }
}
