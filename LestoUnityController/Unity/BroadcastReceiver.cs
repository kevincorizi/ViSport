using UnityEngine;
using System.Collections;

public class BroadcastReceiver : MonoBehaviour
{
    AndroidJavaClass jc;
    string javaMessage = "Ciao";

    void Start()
    {
        Debug.Log("Starting Receiver");
        // Acces the android java receiver we made
        jc = new AndroidJavaClass("com.visport.unitypluginlibrary.UnityReceiver");
        // We call our java class function to create our MyReceiver java object
        jc.CallStatic("createInstance");
        Debug.Log("Started receiver");
    }

    void Update()
    {
        Debug.Log("Update");
        // We get the text property of our receiver
        javaMessage = jc.GetStatic<string>("message");
        Debug.Log("Message: " + javaMessage);
    }
}