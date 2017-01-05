using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainCarController : TrainController
{
    public int trainCarNumber;
    public bool detached;
    public TrainController headTrainEngine;
    public TrainCarController attachedToTrainCar;
    public Queue<int> trainTurnQueue = new Queue<int>();

    private int minTrainCar = 0, maxTrainCar = 3;

    // Use this for initialization
    void Start () {
        health = 50;
	}

    public void activate(TrainController headTrainEngine, TrainCarController attachedToTrainCar, int attachedTrains = 0, string trainCarType = "Mountain")
    {
        this.headTrainEngine = headTrainEngine;
        this.attachedToTrainCar = attachedToTrainCar;
        if (attachedToTrainCar == null)
            trainCarNumber = 1;
        else
            trainCarNumber = attachedToTrainCar.trainCarNumber + 1;
        gameState = headTrainEngine.gameState;//set the game state of attached train;

        attachedTrains--;
        trainTurnQueue = new Queue<int>();

        GameObject tc;
        int rand = Random.Range(minTrainCar, maxTrainCar);
        tc = (GameObject)Resources.Load("traincar_" + rand);
        GetComponent<MeshFilter>().mesh = tc.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().material = tc.GetComponent<Renderer>().sharedMaterial;

        if (attachedTrains > 0)//only called when making multiple train cars at once
        {
            //addOneTrainCar();
            Vector3 pos = transform.position;
            pos += transform.up * 10.5f;

            float angle = Mathf.Acos(5.25f / 20.48f) * Mathf.Rad2Deg;
            pos = new Vector3(transform.position.x + Mathf.Cos((transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f, transform.position.y, transform.position.z + Mathf.Sin((transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f);
            attachedTrainCar = (TrainCarController)Instantiate(this, pos, Quaternion.Euler(270, transform.rotation.eulerAngles.y + 29.3f, 0));
            attachedTrainCar.activate(headTrainEngine, this, attachedTrains);
        }

        minSpeed = headTrainEngine.minSpeed;
    } 

    public void enqueueTurn(int turn)
    {
        //print("turn enqueue " + turn);
        trainTurnQueue.Enqueue(turn);
    }

    public int dequeueTurn()
    {
        int d = trainTurnQueue.Dequeue();
        //print(d);
        return d;
    }

    public void Detatch()
    {
        detached = true;
        Brake(true);
        headTrainEngine = null;

        if (attachedTrainCar)//detatch train cars attached to it
        {
            attachedTrainCar.Detatch();
            //attachedTrainCar = null;
        }
    }

    public void addOneTrainCar(TrainCarController trainCar)
    {
        if (!trainCar)
            return;
        if (attachedTrainCar && !attachedTrainCar.detached)
        {
            attachedTrainCar.addOneTrainCar(trainCar);
        }
        else
        {
            Vector3 pos;
            float rotationChange = 0;
            if (TurnState == 1)
            {
                rotationChange = 29.3f;
                float angle = Mathf.Acos(5.25f / 20.48f) * Mathf.Rad2Deg;
                pos = new Vector3(transform.position.x + Mathf.Cos(-(transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f, transform.position.y, transform.position.z + Mathf.Sin(-(transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f);
            }
            else if (TurnState == -1)
            {
                rotationChange = -29.3f;
                float angle = Mathf.Acos(5.25f / 20.48f) * Mathf.Rad2Deg;
                pos = new Vector3(transform.position.x + Mathf.Cos((transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f, transform.position.y, transform.position.z + Mathf.Sin((transform.eulerAngles.y + angle) * (Mathf.PI / 180)) * 10.5f);
            }
            else
            {
                pos = transform.position;
                pos += transform.up * 10.5f;
            }
            attachedTrainCar = (TrainCarController)Instantiate(this, pos, Quaternion.Euler(270, transform.rotation.eulerAngles.y - rotationChange, 0));
            attachedTrainCar.activate(headTrainEngine, this, 1);
        }
    }

    public void addCoal(int amount)
    {
        headTrainEngine.coal += amount;
    }

    public bool isattachedTo(GameObject trainCar)
    {
        if (attachedToTrainCar)
        {
            if (trainCar == attachedToTrainCar.gameObject)
            {
                return true;
            }
            else
            {
                return attachedToTrainCar.isattachedTo(trainCar);
            }
        }
        else
            return false;
    }

    public void changeHeadTrain(TrainController newHead)
    {
        headTrainEngine = newHead;
        /*if (newHead == this)
        {
            headTrainEngine = null;
        }*/
        if (attachedTrainCar)
            attachedTrainCar.changeHeadTrain(newHead);
    }

    public override bool CoalTrigger(Collider other)
    {
        if (other.name.StartsWith("P_Coal_Black"))
        {
            if (headTrainEngine != null && headTrainEngine.tag == "MainPlayer")
            {
                Destroy(other.gameObject);
                AkCallEvent("Coal_collect");
                addCoal(1);
            }
            else if(gameState != 1)//if not coal collecting mode
            {
                Destroy(other.gameObject);
                addCoal(1);
            }
            return true;
        }
        return false;
    }

    public override bool TrainCarTrigger(Collider other)
    {
        if (other.name.StartsWith("P_TrainCar"))
        {
            TrainCarController enemy = (TrainCarController)other.gameObject.GetComponent(typeof(TrainCarController));
            if (!enemy.detached && !enemy.destroyed && !isattached(other.gameObject) && !isattachedTo(other.gameObject))//is the enemy detatched, or destroyed, or attatched to this train, or is this train attatched to it
            {
                //print("crash " + other.gameObject + " " + gameObject);
                DestroyTrain(other.gameObject);//destroy this train and the explosion source is the other traincar
            }
            return true;
        }
        return false;
    }
}
