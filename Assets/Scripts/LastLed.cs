using UnityEngine;
using System.Collections;

public class LastLed : MonoBehaviour {

    public GameObject previousLed = null;
    public float waitingTime = 0;
    public bool last_is_active = false;
    

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.GetComponent<Light>().enabled == true) {
            //last_is_active = true;
            GameObject[] garray = GameObject.FindGameObjectsWithTag("Led");
            foreach (GameObject go in garray) {
                go.GetComponent<Light>().enabled = false;
            }

        }

       // if (previousLed.GetComponent<Light>().enabled == true)
        //{
        //    StartCoroutine(ActivateLed());
            
            
        //}
        //if (last_is_active == true)
        //{
        //    StartCoroutine(Blinking());
        //    StartCoroutine(Blinking());
        //    gameObject.GetComponent<Light>().enabled = false;
        //    last_is_active = false;
        //}



    }

    //IEnumerator ActivateLed()
    //{
    //    yield return new WaitForSeconds(waitingTime);
    //    gameObject.GetComponent<Light>().enabled = true;
    //    last_is_active = true;


    //}
    //IEnumerator Blinking()
    //{
    //    gameObject.GetComponent<Light>().enabled = false;
    //    yield return new WaitForSeconds(1);
    //    gameObject.GetComponent<Light>().enabled = true;
    //}
}
