using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

     public bool ballReachedPins = false;

     private PinMaster pinMaster;
     private Ball ball;
     
     private float stillnessTimer = 0;
     private const float stillnessTimeout = 5;

     private int bowlCounter = 0;
     private int[] scores = new int[21];

     private enum EGameState {
          TUTORIAL,
          PRE_LAUNCH,
          LAUNCH,
          COUNTING,
          CLEANUP,
          GAME_OVER
     }

     private EGameState state;

	// Use this for initialization
	void Start () {
          state = EGameState.PRE_LAUNCH;
          pinMaster = GameObject.FindObjectOfType<PinMaster>();
          ball = GameObject.FindObjectOfType<Ball>();

          for (int i = 0; i < 20; i++)
               scores[i] = 0;
	}
	
	// Update is called once per frame
	void Update () {
	      switch(state) {
               case EGameState.PRE_LAUNCH:
                    if (ball.InPlay) {
                         state = EGameState.LAUNCH;
                    }
                    break;
               case EGameState.LAUNCH:
                    if (ballReachedPins) {
                         state = EGameState.COUNTING;
                    }
                    break;
               case EGameState.COUNTING:
                    UpdateCounting();
                    break;
               case EGameState.CLEANUP:
                    UpdateCleanup();
                    break;
               default:
                    break;
          }
	}

     private void UpdateCounting() {
          if (ballReachedPins) {
               stillnessTimer += Time.deltaTime;
               if (stillnessTimer > stillnessTimeout || pinMaster.AllPinsStill()) {
                    scores[bowlCounter] = pinMaster.GetFallenPins().Count;
                    LogScore();

                    if (bowlCounter == 20) {
                         //Todo: gameover
                    } else if (bowlCounter >= 18 && scores[bowlCounter] == 10) { // Handle last-frame special cases
                         NextLaunch(true);
                    } else if (bowlCounter == 19) {
                         if (scores[18] == 10 && scores[19] == 0) {
                              NextLaunch(false);
                         } else if (scores[18] + scores[19] == 10) {
                              NextLaunch(true);
                         } else if (scores[18] + scores[19] >= 10) {  // Roll 21 awarded
                              NextLaunch(false);
                         } else {
                              // Todo: gameover
                         }
                    } else if (bowlCounter % 2 == 0) { // First bowl of frame
                         if (scores[bowlCounter] == 10) {
                              bowlCounter++;
                              NextLaunch(true);
                         } else {
                              NextLaunch(false);
                         }
                    } else { // Second bowl of frame
                         NextLaunch(true);
                    }
               }
          }
     }

     /*
      *   This should in theory wait for the pin cleaner to do its stuff, then
      *   reset all game items. For now it just resets.
      */
     private void UpdateCleanup() {
          bowlCounter++;
          ball.Reset();
          ballReachedPins = false;
          stillnessTimer = 0;
          state = EGameState.PRE_LAUNCH;
     }

     private void NextLaunch(bool cleanAll) {
          if (cleanAll) {
               pinMaster.CleanAllPins();
          } else {
               pinMaster.CleanAllFallen();
          }
          state = EGameState.CLEANUP;
     }

     private void LogScore() {
          string output = "";
          foreach (int s in scores) {
               output += s + " ";
          }
          Debug.Log(output);
     }
}
