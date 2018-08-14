using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {

    public enum TileType
    {
        Blocked = -2,
        Starter = -1,
        Empty = 0,
    };

    public int Index;
    public int CurrentType = (int)TileType.Empty;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetCurrentType(int type)
    {
        CurrentType = type;
        if(CurrentType == (int)TileType.Blocked)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.color = Color.black;
            }
        }
        if (CurrentType == (int)TileType.Empty)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.color = Color.white;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHandler player = collider.gameObject.GetComponent<PlayerHandler>();

        if (player)
        {
            if (CurrentType != player.GetPlayerNumber())
            {
                // If !startTIle
                if (CurrentType != (int)TileType.Starter)
                {
                    // Give player a point
                    player.AddPoints(1);

                    // Set CurrentType to player number
                    CurrentType = player.GetPlayerNumber();

                    // Set player object to enter tile complete
                    player.SetDestinationPoint(gameObject.transform.position);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        PlayerHandler player = collider.gameObject.GetComponent<PlayerHandler>();

        if (player)// && CurrentType != player.GetPlayerNumber())
        {
            // If !startTIle
            if (CurrentType != (int)TileType.Starter)
            {
                // Set CurrentType to player number
                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                if(renderer)
                {
                    renderer.color = player.GetColor();
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        PlayerHandler player = collider.gameObject.GetComponent<PlayerHandler>();

        if (player)// && CurrentType != player.GetPlayerNumber())
        {
            // If !startTIle
            if (CurrentType != (int)TileType.Starter)
            {
                // Set CurrentType to player number
                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                if (renderer)
                {
                    renderer.color = player.GetColor();
                }
            }
        }
    }
}
