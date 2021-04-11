using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public StateManager stateManager;

    public float speed;
    int facingDirection; //using int to show direction: number shows in clockwise, where [1] is up, [3] is right, [5] is down, and [7] is left
    public Vector2 dir;

    Rigidbody2D myBody;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector2(0, -1);
        facingDirection = 5;
        myBody = gameObject.GetComponent<Rigidbody2D>();
        dir = new Vector2(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (stateManager.getCurrentState() == stateManager.state_normal)
        {
            checkDir();
            playerMovement();
        }
    }

    public void checkDir()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir = new Vector2(0, -1);
            facingDirection = 5;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dir = new Vector2(-1, -1);
                facingDirection = 6;
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dir = new Vector2(1, -1);
                facingDirection = 4;
            }
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir = new Vector2(-1, 0);
            facingDirection = 7;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                dir = new Vector2(-1, 1);
                facingDirection = 8;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dir = new Vector2(-1, -1);
                facingDirection = 6;
            }
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir = new Vector2(0, 1);
            facingDirection = 1;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dir = new Vector2(-1, 1);
                facingDirection = 8;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dir = new Vector2(1, 1);
                facingDirection = 2;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir = new Vector2(1, 0);
            facingDirection = 3;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                dir = new Vector2(1, 1);
                facingDirection = 2;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dir = new Vector2(1, -1);
                facingDirection = 4;
            }
        } else
        {
            facingDirection = 0;
        }
    }

    void playerMovement()
    {
        switch (facingDirection)
        {
            case 1:
                myBody.velocity = new Vector3(0, 1) * speed;
                break;

            case 3:
                myBody.velocity = new Vector3(1, 0) * speed;
                break;

            case 5:
                myBody.velocity = new Vector3(0, -1) * speed;
                break;

            case 7:
                myBody.velocity = new Vector3(-1, 0) * speed;
                break;

            case 2:
                myBody.velocity = new Vector3(0.75f, 0.75f) * speed;
                break;

            case 4:
                myBody.velocity = new Vector3(0.75f, -0.75f) * speed;
                break;

            case 6:
                myBody.velocity = new Vector3(-0.75f, -0.75f) * speed;
                break;

            case 8:
                myBody.velocity = new Vector3(-0.75f, 0.75f) * speed;
                break;

            case 0:
                myBody.velocity = new Vector2(0, 0) * speed;
                break;
        } 

        //myBody.velocity = dir * speed;
    }
}
