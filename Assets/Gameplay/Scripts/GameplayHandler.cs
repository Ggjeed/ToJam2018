using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour {

    public TileMapHandler TileMap;
    public List<GameObject> Players;
    //public PowerUpHandler powerUps;

    public EndGameHandler endGameScreen;

    public List<Text> PlayerPointsUI;
    public Text GameTimerUI;

    public float GameTimer = 0.0f;
    public float PowerUpTimer = 0.0f;

    private float gameTimer = 0.0f;
    private float powerUpTimer = 0.0f;
    private bool gameStarted = false;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if(!gameStarted)
        {
            powerUpTimer = PowerUpTimer;
            gameTimer = GameTimer;

            if (TileMap)
            {
                TileMap.Initialize(Players);
            }

            gameStarted = true;
            return;
        }

        gameTimer -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer - (minutes * 60));
        GameTimerUI.text = (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());

        powerUpTimer -= Time.deltaTime;

        if(powerUpTimer < 0.0f)
        {
            // Spawn power up
            powerUpTimer = PowerUpTimer;
        }

        if(gameTimer < 0.0f)
        {
            // End the game
            //ResetGame();
            int winningPlayer = 0;
            int highestPoints = 0;
            for (int index = 0; index < Players.Count; ++index)
            {
                PlayerHandler handler = Players[index].GetComponent<PlayerHandler>();
                if (handler)
                {
                    if (handler.GetPoints() > highestPoints)
                    {
                        winningPlayer = index;
                        highestPoints = handler.GetPoints();
                    }
                }
            }
            endGameScreen.ShowPlayerWins(winningPlayer);
        }
    }

    public void UpdatePlayerPoints(int playerNum, int points)
    {
        if(playerNum > PlayerPointsUI.Count)
        {
            Debug.LogWarning("Player number is higher than number of player UI elements!");
            return;
        }

        PlayerPointsUI[playerNum-1].text = points.ToString();
    }

    public void ResetGame()
    {
        endGameScreen.HideUI();
        for(int index = 0; index < Players.Count; ++index)
        {
            PlayerHandler handler = Players[index].GetComponent<PlayerHandler>();
            if(handler)
            {
                handler.ResetPlayerPoints();
            }
        }
        gameStarted = false;
    }
}
