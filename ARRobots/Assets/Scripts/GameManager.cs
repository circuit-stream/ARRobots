using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public RobotTouchController robotTouchController;

    public int lives = 10;

    public Canvas gameOverCanvas;

    public TMP_Text livesText;

    void Awake()
    {
        instance = this;

        gameOverCanvas.gameObject.SetActive(false);

        livesText.text = lives.ToString();
    }
    
    public void LostLives()
    {
        // decrease lives by one
        lives--;

        livesText.text = lives.ToString();

        if (lives <= 0)
        {
            // game over
            gameOverCanvas.gameObject.SetActive(true);
        }
        
    }

    public void RestartGame()
    {
        lives = 10;
        gameOverCanvas.gameObject.SetActive(false);
    }
}
