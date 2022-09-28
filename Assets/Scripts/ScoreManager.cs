using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text winText;
    int winAmount = 5;
    int score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        scoreText.text = "Score: " + score.ToString();
        winText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= winAmount)
        {
            winText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        Debug.Log(score);

        scoreText.text = "Score: " + score;
    }
}
