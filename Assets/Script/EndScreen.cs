using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    Scorekeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = FindObjectOfType<Scorekeeper>();    
    }

    public void ShowFinalScore()
    {
        finalScoreText.text ="Congratulations!\nYou got a score of " + 
                                scoreKeeper.CalculateScore() + " %";

    }

}
