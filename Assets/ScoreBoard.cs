using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    private static int score;

    private static float scratchCooldownTime = 0.5f;
    private static float scratchCooldown = 0;
    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (value < score)
            {
                scratchCooldown = scratchCooldownTime;
            }
            score = value;
        }
    }

    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE : " + Score;
        if (scratchCooldown > 0)
        {
            if (scoreText.color != Color.red)
            {
                scoreText.color = Color.red;
            }
            scratchCooldown -= Time.deltaTime;
        }
        else if(scoreText.color != Color.white)
        {
            scoreText.color = Color.white;
        }
    }
}
