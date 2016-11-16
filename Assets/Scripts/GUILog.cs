using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUILog : MonoBehaviour {

     private Text guitext;

     public string output = "";
     public string stack = "";
     void OnEnable() {
          Application.logMessageReceived += HandleLog;
     }
     void OnDisable() {
          Application.logMessageReceived -= HandleLog;
     }
     void HandleLog(string logString, string stackTrace, LogType type) {
          output = logString;
          stack = stackTrace;
     }

     void Start() {
          guitext = GetComponent<Text>();
     }

     void Update() {
          guitext.text = output;
     }
}
