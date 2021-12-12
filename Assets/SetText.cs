using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    public Text endGameText;
    private int[] scores;
    private int[] ladder;

    private bool already_ranked(int i, int j)
    {
        for (int k = i - 1; k >= 0; k--)
            if (ladder[k] == j)
                return true;
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        endGameText.text = "Results:\n";
        scores = new int[] {  GameManager.playersScores[0],
                                    GameManager.playersScores[1],
                                    GameManager.playersScores[2],
                                    GameManager.playersScores[3] };
        Array.Sort(scores);
        ladder = new int[] { 0, 0, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (scores[i] == GameManager.playersScores[j]
                    && (i == 0 || already_ranked(i, j) == false))
                {
                    ladder[i] = j;
                    break;
                }
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
