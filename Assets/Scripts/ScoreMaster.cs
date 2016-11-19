using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreMaster : MonoBehaviour {

     public List<int> scores = new List<int>();

     private List<string> formattedScores = new List<string>();
     private int[] scoresArray = new int[21];
     private bool dirty = true;

     public void AddScore(int score) {
          scores.Add(score);
          dirty = true;
     }

     public int[] GetScores() {
          scores.CopyTo(scoresArray);
          return scoresArray;
     }

	// Use this for initialization
	void Start () {
          ResetFrames();
	}
	
	// Update is called once per frame
	void Update () {
	      if (dirty) {
               FormatScores();
               UpdateFrames();
               string output = "";
               foreach (string s in formattedScores)
                    output += s + " ";
               Debug.Log(output);
               dirty = false;
          }
	}

     public void ResetScores() {
          scores.Clear();
          formattedScores.Clear();
     }

     public void ResetFrames() {
          int i = 0;
          foreach (Transform child in transform) {
               foreach (Transform score in child) {
                    score.gameObject.GetComponent<Text>().text = "";
               }
          }
     }

     private void UpdateFrames() {
          int[] totalScores = new int[10];
          List<int> frameScores = ScoreFrames();

          for (int i = 0; i < 10; i++) {
               Transform child = transform.GetChild(i);

               if (i == 9) {
                    Text a = child.transform.GetChild(0).gameObject.GetComponent<Text>();
                    Text b = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                    Text c = child.transform.GetChild(2).gameObject.GetComponent<Text>();
                    Text tot = child.transform.GetChild(3).gameObject.GetComponent<Text>();

                    int scorePos = 18;

                    if (scorePos + 1 > formattedScores.Count) {
                         return;
                    } else {
                         a.text = formattedScores[scorePos];
                    }
                    if (scorePos + 2 > formattedScores.Count) {
                         return;
                    } else {
                         b.text = formattedScores[scorePos + 1];

                         if (scores[scorePos] + scores[scorePos + 1] >= 10) {
                              if (scorePos + 3 <= formattedScores.Count) {   // 21st bowl awarded
                                   c.text = formattedScores[scorePos + 2];
                                   tot.text = frameScores[i].ToString();
                              }
                         } else {
                              tot.text = frameScores[i].ToString();
                         }
                    }
               } else {
                    Text a = child.transform.GetChild(0).gameObject.GetComponent<Text>();
                    Text b = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                    Text tot = child.transform.GetChild(2).gameObject.GetComponent<Text>();

                    int scorePos = 2 * i;
                    if (scorePos + 1 > formattedScores.Count)
                         return;
                    else {
                         a.text = formattedScores[scorePos];
                    }
                    if (scorePos + 2 > formattedScores.Count)
                         return;
                    else {
                         b.text = formattedScores[scorePos + 1];
                         if (i < frameScores.Count)
                              tot.text = frameScores[i].ToString();
                    }
               }    
          }
     }

     private List<int> ScoreFrames() {
          List<int> frames = new List<int>();
          int prevScore;
          int newScore = 0;

          // Index i points to 2nd bowl of frame
          for (int i = 1; i < scores.Count; i += 2) {
               if (frames.Count == 10) {
                    break;
               }                // Prevents 11th frame score

               prevScore = frames.Count > 0 ? frames[frames.Count - 1] : 0;

               if (scores[i - 1] + scores[i] < 10) {              // Normal "OPEN" frame
                    newScore = scores[i - 1] + scores[i];
                    frames.Add(newScore + prevScore);
               }

               if (scores.Count - i <= 1) { break; }             // Ensure at least 1 look-ahead available

               try {
                    if (scores[i - 1] == 10) {
                         // STRIKE frame has just one bowl
                         if (scores[i + 1] == 10) {
                              if (i == 19)
                                   newScore = 30;
                              else if (i == 17)
                                   newScore = 20 + scores[i + 2];
                              else
                                   newScore = 20 + scores[i + 3];
                         } else {
                              newScore = 10 + scores[i + 1] + scores[i + 2];
                         }
                    } else if (scores[i - 1] + scores[i] == 10) {      // SPARE bonus
                         newScore = 10 + scores[i + 1];
                    }
               } catch (System.ArgumentOutOfRangeException e) {
                    // Tried to evaluate a SPARE or STRIKE not yet available, break
                    break;
               }
               frames.Add(prevScore + newScore);
          }
          return frames;
     }

     private void FormatScores() {
          formattedScores.Clear();

          for (int i = 0; i < scores.Count; i++) {
               int box = i + 1;                   // Score box 1 to 21 

               if (scores[i] == 0) {                                 // Always enter 0 as -
                    formattedScores.Add("-");
               } else if ((box % 2 == 0 || box == 21) && scores[i - 1] + scores[i] == 10) {   // SPARE
                    formattedScores.Add("/");
               } else if (box >= 19 && scores[i] == 10) {                // STRIKE in frame 10
                    formattedScores.Add("X");
               } else if (scores[i] == 10) {                         // STRIKE in frame 1-9
                    formattedScores.Add("X");
                    formattedScores.Add("");
                    i++;
               } else {
                    formattedScores.Add(scores[i].ToString());                      // Normal 1-9 bowl
               }
          }
     }
}
