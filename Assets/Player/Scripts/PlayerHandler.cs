using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {
    
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    };

    public int Speed;
    public int PlayerNumber = 0;
    public Color PlayerColor = Color.white;
    public string JoystickName;

    private Vector3 destinationPoint = new Vector3();
    private bool hasDestination = false;
    private int playerPoints = 0;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPlayerPoints()
    {
        playerPoints = 0;
        GameplayHandler gph = GameObject.FindWithTag("GameController").GetComponent<GameplayHandler>();
        if (gph)
        {
            gph.UpdatePlayerPoints(PlayerNumber, playerPoints);
        }
    }
	
	// Update is called once per frame
	void Update () {

        float horizontal = Input.GetAxis(JoystickName + "_Horizontal");
        float vertical = Input.GetAxis(JoystickName + "_Vertical");
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);

        if (hasDestination)
        {
            movement = destinationPoint - gameObject.transform.position;
        }
        else if (Mathf.Abs(horizontal) == 1.0f)
        {
            movement.x = horizontal;
        }
        else if (Mathf.Abs(vertical) == 1.0f)
        {
            movement.y = vertical;
        }

        movement = movement.normalized * Speed;

        //Debug.Log("Movement = " + movement.ToString());
        if (movement.magnitude >= 1)
        {
            Vector3 newPosition = gameObject.transform.position + movement * Time.deltaTime;

            // Check status on Tilemap
            TileMapHandler tlh = GameObject.FindWithTag("Tilemap").GetComponent<TileMapHandler>();
            if (tlh)
            {
                Vector3 newPositionWithBorder = newPosition;
                // Have to calculate for position of "forward" edge based on direction
                if(movement.x > 0)
                {
                    newPositionWithBorder.x += 32/2;
                    SetDirection((int)Direction.Right);
                }
                else if(movement.x < 0)
                {
                    // do nothing because we're already at the border on the left side
                    SetDirection((int)Direction.Left);
                }
                if (movement.y > 0)
                {
                    newPositionWithBorder.y += 32/2;
                    SetDirection((int)Direction.Up);
                }
                else if (movement.y < 0)
                {
                    // do nothing because we're already at the border on the bottom
                    SetDirection((int)Direction.Down);
                }

                //Debug.Log("Destination Tile Type = " + ((TileHandler.TileType)tlh.GetTileScript(newPosition).CurrentType).ToString());
                if (tlh.GetTileScript(newPositionWithBorder).CurrentType != (int)TileHandler.TileType.Blocked)
                {
                    gameObject.transform.position = newPosition;
                }
                else
                {
                    Debug.Log("Tile was blocked. Index = " + tlh.GetTileIndex(newPositionWithBorder) + ", Attempted Position(w/ border) = " + newPositionWithBorder.ToString());
                }
            }

            float magnitude = (transform.position - destinationPoint).magnitude;
            if (hasDestination && (magnitude < 5.0f))
            {
                transform.position = destinationPoint;
                hasDestination = false;
            }
        }
        else
        {
            StopAnimation();
        }
	}

    public void SetDestinationPoint(Vector3 position)
    {
        hasDestination = true;
        destinationPoint = position;
    }

    // pass negative value to reduce points
    public void AddPoints(int points)
    {
        playerPoints += points;
        GameplayHandler gph = GameObject.FindWithTag("GameController").GetComponent<GameplayHandler>();
        if(gph)
        {
            gph.UpdatePlayerPoints(PlayerNumber, playerPoints);
        }
    }

    public int GetPoints()
    {
        return playerPoints;
    }

    public int GetPlayerNumber()
    {
        return PlayerNumber;
    }

    public Color GetColor()
    {
        return PlayerColor;
    }

    private void PlayAnimation(string clip)
    {
        gameObject.GetComponent<Animator>().Play(clip);
    }

    private void SetDirection(int direction)
    {
        if(gameObject.GetComponent<Animator>().GetInteger("Direction") != direction)
        {
            gameObject.GetComponent<Animator>().SetInteger("Direction", direction);
        }
    }

    private void StopAnimation()
    {
        //gameObject.GetComponent<Animator>().Stop();
    }
}
