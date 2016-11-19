using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

     public bool ballReachedPins = false;

     private ScoreMaster scoreMaster;
     private PinMaster pinMaster;
     private Ball ball;
     
     private float stillnessTimer = 0;
     private const float STILLNESS_TIMEOUT = 15;
     private const float STILLNESS_MIN = 6;

     private float cleanupTimer = 0;
     private const float CLEANUP_TIMEOUT = 2;

     private int bowlCounter = 0;

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
          scoreMaster = GameObject.FindObjectOfType<ScoreMaster>();
          pinMaster = GameObject.FindObjectOfType<PinMaster>();
          ball = GameObject.FindObjectOfType<Ball>();
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
               if (stillnessTimer > STILLNESS_TIMEOUT ||    // Max time elapsed
                    (stillnessTimer > STILLNESS_MIN && pinMaster.AllPinsStill())) {  // Min time elapsed and all pins are still
                    scoreMaster.AddScore(pinMaster.GetFallenPins().Count);
                    int[] scores = scoreMaster.GetScores();
                    LogScore(scores);

                    if (bowlCounter == 20) {
                         GameOver();
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
                              GameOver();
                         }
                    } else if (bowlCounter % 2 == 0) { // First bowl of frame
                         if (scores[bowlCounter] == 10) {
                              bowlCounter++;
                              scoreMaster.AddScore(0); // Virtual 0 after strike
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
          if (cleanupTimer < CLEANUP_TIMEOUT) {
               cleanupTimer += Time.deltaTime;
          } else {
               cleanupTimer = 0;
               bowlCounter++;
               pinMaster.LowerAll();
               ball.Reset();
               ballReachedPins = false;
               state = EGameState.PRE_LAUNCH;
          }
     }

     private void NextLaunch(bool cleanAll) {
          if (cleanAll) {
               pinMaster.CleanAllPins();
          } else {
               pinMaster.CleanAllFallen();
          }
          pinMaster.RaiseAll();
          stillnessTimer = 0;
          state = EGameState.CLEANUP;
     }

     private void GameOver() {
          ball.Reset();
          ball.Lock();
          state = EGameState.GAME_OVER;
     }

     private void LogScore(int[] scores) {
          string output = "";
          foreach (int s in scores) {
               output += s + " ";
          }
          Debug.Log(output);
     }
}
