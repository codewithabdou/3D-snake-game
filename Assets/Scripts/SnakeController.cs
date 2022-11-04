using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private int direction;
    private List<Transform> snakeParts;
    private Rigidbody snakeBody;
    private float stepLength = 0.18f;
    private float counterToMove = 0;
    private bool move = false;
    private Transform tr;
    private float movementFrequency = 0.1f;
    private List<Vector3> dMovement;
    [SerializeField]
    private GameObject tail;
    private bool createNodeAtTail;
    private bool gameEnded = false;

    void Awake()
    {
        tr = transform;
        snakeBody = GetComponent<Rigidbody>();
        dMovement = new List<Vector3>()
        {
            new Vector3(-stepLength,0f),
            new Vector3(stepLength,0f),
            new Vector3(0f,stepLength),
            new Vector3(0f,-stepLength),

        };
        InitSnake();
    }

    int RandomDirection()
    {
        return (Random.Range(0, Directions.COUNT));
    }

    void InitSnake()
    {
        snakeParts = new List<Transform>();
        for (int i = 0; i < 3; i++)
            snakeParts.Add(tr.GetChild(i).GetComponent<Transform>());
        direction = RandomDirection();
        snakeParts[0].position = tr.position;

        switch (direction)
        {
            case Directions.LEFT:
                snakeParts[0].Rotate(0f, 0f, 180f);
                for (int i = 1; i < 3; i++)
                {
                    snakeParts[i].Rotate(0f, 0f, 180f);
                    snakeParts[i].position = snakeParts[i - 1].position + new Vector3(Metrics.NODE_NODE, 0f, 0f);
                }
                break;

            case Directions.RIGHT:
                snakeParts[0].Rotate(0f, 0f, 0f);
                for (int i = 1; i < 3; i++)
                {
                    snakeParts[i].Rotate(0f, 0f, 0f);
                    snakeParts[i].position = snakeParts[i - 1].position - new Vector3(Metrics.NODE_NODE, 0f, 0f);
                }

                break;

            case Directions.UP:
                snakeParts[0].Rotate(0f, 0f, 90f);
                for (int i = 1; i < 3; i++)
                {
                    snakeParts[i].Rotate(0f, 0f, 90f);
                    snakeParts[i].position = snakeParts[i - 1].position - new Vector3(0f, Metrics.NODE_NODE, 0f);
                }
                break;

            case Directions.DOWN:
                snakeParts[0].Rotate(0f, 0f, -90f);
                for (int i = 1; i < 3; i++)
                {
                    snakeParts[i].Rotate(0f, 0f, -90f);
                    snakeParts[i].position = snakeParts[i - 1].position + new Vector3(0f, Metrics.NODE_NODE, 0f);
                }
                break;
        }
        move = false;

    }

    private void MovementCounter()
    {
        if (!gameEnded)
        {
            counterToMove += Time.deltaTime;
            if (counterToMove >= movementFrequency)
            {
                counterToMove = 0;
                move = true;
            }
        }
    }

    private void Move()
    {
        move = false;
        Vector3 prevTrans = snakeParts[0].position;
        snakeBody.position += dMovement[direction]; 
        snakeParts[0].position += dMovement[direction];
        Vector3 nextTrans;
        for (int i =1; i < snakeParts.Count; i++)
        {
            nextTrans = snakeParts[i].position;
            snakeParts[i].position = prevTrans;
            prevTrans = nextTrans;
        }

        if (createNodeAtTail)
        {
            createNodeAtTail = false;
            GameObject newNode = Instantiate(tail, snakeParts[snakeParts.Count-1].position, snakeParts[0].rotation);
            newNode.transform.SetParent(tr);
            snakeParts.Add(newNode.transform);

        }





    }

    void PlayerMovementListenner()
    {
        float movementH = Input.GetAxisRaw("Horizontal");
        float movementV = Input.GetAxisRaw("Vertical");
        if(movementH!=0 && movementV == 0)
        {
            if (movementH < 0)
            {
                if (direction != Directions.LEFT && direction != Directions.RIGHT)
                {
                    int rotationDegree = 1;
                    if (direction == Directions.DOWN)
                        rotationDegree = -1;
                    foreach (Transform Node in snakeParts)
                    {
                        Node.Rotate(0f, 0f, rotationDegree * 90f);
                    }
                    direction = Directions.LEFT;
                    ForceMove();

                }

            }
            else
            {
                if (direction != Directions.LEFT && direction != Directions.RIGHT)
                {
                    int rotationDegree = 1;
                    if (direction == Directions.UP)
                        rotationDegree = -1;
                    foreach (Transform Node in snakeParts)
                    {
                        Node.Rotate(0f, 0f, rotationDegree * 90f);
                    }
                    direction = Directions.RIGHT;
                    ForceMove();
                }

            }

        }
        else if (movementH == 0 && movementV != 0)
        {
            if (movementV < 0)
            {
                if (direction != Directions.UP && direction != Directions.DOWN)
                {
                    int rotationDegree = 1;
                    if (direction == Directions.RIGHT)
                        rotationDegree = -1;
                    foreach (Transform Node in snakeParts)
                    {
                        Node.Rotate(0f, 0f, rotationDegree * 90f);
                    }
                    direction = Directions.DOWN;
                    ForceMove();
                }
            }
            else
            {
                if (direction != Directions.UP && direction != Directions.DOWN)
                {
                    int rotationDegree = 1;
                    if (direction == Directions.LEFT)
                        rotationDegree = -1;
                    foreach (Transform Node in snakeParts)
                    {
                        Node.Rotate(0f, 0f, rotationDegree * 90f);
                    }
                    direction = Directions.UP;
                    ForceMove();
                }
            }

        }
    }

    void ForceMove()
    {
        counterToMove = 0;
        move = false;
        Move();

    }

    // Update is called once per frame
    void Update()
    {
        MovementCounter();
        PlayerMovementListenner();
    }

    private void FixedUpdate()
    {
        if (move)
            Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(Tags.FRUIT))
        {
            Destroy(target.gameObject);
            createNodeAtTail = true;
            GameplayController.Instance.updateScore();
            AudioManager.Instance.playSound("eat");
        }

        if(target.CompareTag(Tags.WALL) || target.CompareTag(Tags.BOMB) || target.CompareTag(Tags.TAIL))
        {
            gameEnded = true;
            StartCoroutine(GoToMainMenu());
            AudioManager.Instance.playSound("death");
        }


    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(3.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
