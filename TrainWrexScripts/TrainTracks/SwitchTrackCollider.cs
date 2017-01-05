using UnityEngine;
using System.Collections;

public class SwitchTrackCollider : MonoBehaviour {

	public bool straight, left, right;
    public bool startStraight, startLeft, startRight;
    private bool activeStraight, activeLeft, activeRight;
    private Material leftTurnMesh, rightTurnMesh, straightMesh;
    private Vector3 leftTurnPoint, rightTurnPoint;

    // Use this for initialization
    void Start () {
        if (!startStraight && !startLeft && !startRight)//if not start position is choosen then set defult
        {
            if (straight)//if straight is possible set that active
                startStraight = true;
            else if (left)//if straight is not possible then if left is possible set that as starting position
                startLeft = true;
            else if (right)//if straight is not possible then if left is not possible and right is possible set start possition right
                startRight = true;
        }

        leftTurnMesh = (Material)Resources.Load("M_Arrow_Left");
        rightTurnMesh = (Material)Resources.Load("M_Arrow_Right");
        straightMesh = (Material)Resources.Load("M_Arrow");

        float rotationRadians = (transform.eulerAngles.y * (Mathf.PI / 180));
        leftTurnPoint = new Vector3(transform.position.x + (Mathf.Sin(rotationRadians) * 20.48f) - (Mathf.Cos(rotationRadians) * (transform.localScale.x / 2)),
                                                    transform.position.y,
                                                    transform.position.z + (Mathf.Cos(rotationRadians) * 20.48f) + (Mathf.Sin(rotationRadians) * (transform.localScale.x / 2)));

        rightTurnPoint = new Vector3(transform.position.x - (Mathf.Sin(rotationRadians) * 20.48f) - (Mathf.Cos(rotationRadians) * (transform.localScale.x / 2)),
                                                   transform.position.y,
                                                   transform.position.z - (Mathf.Cos(rotationRadians) * 20.48f) + (Mathf.Sin(rotationRadians) * (transform.localScale.x / 2)));

        if (startStraight && straight)
            activeStraight = true;
        if (startLeft && left)
        {
            activeLeft = true;
            ChangeMesh(-1);
        }
        if (startRight && right)
        {
            activeRight = true;
            ChangeMesh(1);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeMesh(int turn)
    {
        if(turn == -1)
            GetComponent<Renderer>().material = leftTurnMesh;
        if (turn == 0)
            GetComponent<Renderer>().material = straightMesh;
        if (turn == 1)
            GetComponent<Renderer>().material = rightTurnMesh;
        
    }

    void OnMouseDown()
    {
        //straight, right, left clockwise
        if (activeStraight)
        {
            if (right)
            {
                activeStraight = false;
                activeRight = true;
                ChangeMesh(1);
            }
            else if (left)
            {
                activeStraight = false;
                activeLeft = true;
                ChangeMesh(-1);
            }
        }
        else if (activeRight)
        {
            if (left)
            {
                activeRight = false;
                activeLeft = true;
                ChangeMesh(-1);
            }
            else if (straight)
            {
                activeRight = false;
                activeStraight = true;
                ChangeMesh(0);
            }
        }
        else if (activeLeft)
        {
            if (straight)
            {
                activeLeft = false;
                activeStraight = true;
                ChangeMesh(0);
            }
            else if (right)
            {
                activeLeft = false;
                activeRight = true;
                ChangeMesh(1);
            }
        }
        print(activeLeft + " " + activeStraight + " " + activeRight);
    }

    void OnTriggerEnter(Collider other){
        if (other.GetComponent<TrainController>() != null && other.GetType() == typeof(BoxCollider))
        {
            if (Vector3.Dot(transform.right.normalized, -other.transform.up.normalized) > 0.5)//faceing eachother
            {
                float rotationRadians = (transform.eulerAngles.y * (Mathf.PI / 180));
                float degree = (transform.eulerAngles.y) * (Mathf.PI / 180) + (3 * Mathf.PI) / 2;
                if (other.GetComponent<TrainCarController>() != null)//its a train car
                {
                    int turn = other.GetComponent<TrainCarController>().dequeueTurn();
                    other.GetComponent<TrainCarController>().enqueueAttatchedTrain(turn);
                    switch (turn)
                    {
                        case -1:
                            other.GetComponent<TrainCarController>().TurnLeft(leftTurnPoint, degree);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(-1);
                            break;
                        case 0:
                            other.GetComponent<TrainCarController>().straightTrack(transform.eulerAngles.y);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(0);

                            break;
                        case 1:
                            other.GetComponent<TrainCarController>().TurnRight(rightTurnPoint, degree);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(1);

                            break;
                    }
                    return;
                }else if (other.GetComponent<TrainEngineMainPlayer>() != null)//its a the mainplayer train
                {
                    int turn = other.GetComponent<TrainEngineMainPlayer>().nextTurn;
                    //other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(turn);
                    if (turn == -1)
                    {
                        if (left)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnLeft(leftTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(-1);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(-1);
                        }else if (straight)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().straightTrack(transform.eulerAngles.y);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(0);
                        }else if (right)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnRight(rightTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(1);
                        }
                        other.GetComponent<TrainEngineMainPlayer>().nextTurn = 0;
                    }
                    else if (turn == 0)
                    {
                        if (straight)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().straightTrack(transform.eulerAngles.y);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(0);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(0);
                        }
                        else if (left)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnLeft(leftTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(-1);
                        }
                        else if (right)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnRight(rightTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(1);
                        }
                        other.GetComponent<TrainEngineMainPlayer>().nextTurn = 0;

                    }
                    else if (turn == 1)
                    {
                        if (right)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnRight(rightTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(1);
                            //other.GetComponent<TrainCarController>().enqueueAttatchedTrain(1);
                        }
                        else if (straight)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().straightTrack(transform.eulerAngles.y);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(0);
                        }
                        else if (left)
                        {
                            other.GetComponent<TrainEngineMainPlayer>().TurnLeft(leftTurnPoint, degree);
                            other.GetComponent<TrainEngineMainPlayer>().enqueueAttatchedTrain(-1);
                        }
                        other.GetComponent<TrainEngineMainPlayer>().nextTurn = 0;
                    }
                    return;
                }
                if (activeStraight)
                {
                    other.GetComponent<TrainController>().straightTrack(transform.eulerAngles.y);
                    other.GetComponent<TrainController>().enqueueAttatchedTrain(0);

                }
                else if (activeRight)
                {
                    other.GetComponent<TrainController>().TurnRight(rightTurnPoint, degree);
                    other.GetComponent<TrainController>().enqueueAttatchedTrain(1);

                }
                else
                {
                    other.GetComponent<TrainController>().TurnLeft(leftTurnPoint, degree);
                    other.GetComponent<TrainController>().enqueueAttatchedTrain(-1);

                }
            }
            return;
        }
	}
}
