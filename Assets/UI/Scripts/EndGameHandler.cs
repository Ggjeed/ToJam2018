using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndGameHandler : MonoBehaviour {

    public List<Image> PlayerWinScreens;

	// Use this for initialization
	void Start () {
        HideUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowPlayerWins(int player)
    {
        if(player > PlayerWinScreens.Count)
        {
            Debug.LogWarning("Player number is greater than number of win screens!");
            return;
        }

        gameObject.SetActive(true);
        for(int index = 0; index < PlayerWinScreens.Count; ++index)
        {
            PlayerWinScreens[index].gameObject.SetActive(false);
        }

        PlayerWinScreens[player].gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
