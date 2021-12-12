using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    public Text endGameText;
    // Start is called before the first frame update
    void Start()
    {
        endGameText.text = "Results:\n";
        int[] scores = new int[] {  GameManager.playersScores[0],
                                    GameManager.playersScores[1],
                                    GameManager.playersScores[2],
                                    GameManager.playersScores[3] };
        Array.Sort(scores);
        int[] ladder = new int[] { 0, 0, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (scores[i] == GameManager.playersScores[j])
                    ladder[i] = j;
            }
        }

        for (int i = 3; i >= 0; i--)
        {
            endGameText.text += "Player" + (ladder[i] + 1).ToString() + ": " + scores[i] + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
