using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highScoreText;
    public string playerName;
    public GameObject GameOverText;
    public GameObject GameOverHighScoreText;
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    public int highScore;
    public string highScoreName;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if(GameData.instance != null)
        {
            //set player name from main menu
            playerName = GameData.instance.playerName;

            //Load high score data
            GameData.instance.LoadGameData();
            highScore = GameData.instance.highScore;
            highScoreName = GameData.instance.highScorePlayerName;
            
            //set score text to include player's name and 0 points.
            ScoreText.text = $"{playerName}'s Score : {m_Points}";
        }

        //if high score data is available, display it. Otherwise, use a boilerplate.
        if (highScoreName != "" && highScore != 0)
        {
            highScoreText.text = $"{highScoreName} has the high score of {highScore} points!";
        }
        else
        {
            highScoreText.text = "No one has placed a high score!";
        }


        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{playerName}'s Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        //Save game. Check for new high score and display special text
        GameData.instance.SaveGame(m_Points, playerName);
        if (m_Points > GameData.instance.highScore)
        {
            GameOverHighScoreText.SetActive(true);
        }
    }

}
