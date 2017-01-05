using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour {

    public int TurnState = 0;//0 straight, -1 left, 1 right
    public float minSpeed = Mathf.PI / 12 * 2, currentSpeed = Mathf.PI / 12 * 2;//pi/12
    public bool brake;
    public float currentDegree;
    private Vector3 turnPoint;
    private float turnRadius = 20.48f;
    private float turnAmount;
    public int health = 100;
    public int coal;
    public bool destroyed;
    public int gameState;//0 coal mode, 1 train wreck, 2 car mode, 3 boss fight

    public TrainCarController attachedTrainCar;

    public GameObject explosion;

    private uint bankID;//for audio bank

    public void AkCallEvent(string ID, bool overrideTag = false)
    {
        if (tag == "MainPlayer" || overrideTag)
            AkSoundEngine.PostEvent(ID, gameObject);
    }

    void OnDestroy()
    {
        AkCallEvent("GAMEPLAY_TRAIN_LOOP_STOP");
    }

    public void Start() {
        print("startTrainController");
        currentDegree = (transform.eulerAngles.y) * (Mathf.PI / 180);
        //turnAmount = currentSpeed / turnRadius;

        AkCallEvent("Gameplay_Train_Loop");
    }

    public void DestroyTrain(Collider other)
    {
        if (attachedTrainCar)
            attachedTrainCar.Detatch();

        Vector3 v3 = other.transform.position;
        v3 += other.transform.right;
        v3.y += 1;
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 15, 0);
        transform.GetComponent<Rigidbody>().AddExplosionForce(200, v3, 10);
        GetComponent<Collider>().isTrigger = false;//only add if you want to hit a sollid surface
        destroyed = true;
        explosion.transform.position = transform.position;//sets explosion to this trains current location
        Instantiate(explosion);
    }

    public void DestroyTrain(GameObject other)
    {
        if (attachedTrainCar)
            attachedTrainCar.Detatch();

        Vector3 v3 = other.transform.position;
        v3 += other.transform.right;
        v3.y += 1;
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 15, 0);
        transform.GetComponent<Rigidbody>().AddExplosionForce(200, v3, 10);
        GetComponent<Collider>().isTrigger = false;//only add if you want to hit a sollid surface
        destroyed = true;
        explosion.transform.position = transform.position;//sets explosion to this trains current location
        Instantiate(explosion);
    }

    public void heal(int h)
    {
        if (health + h < 100)
        {
            health += h;
        }
        else if (attachedTrainCar)
        {
            attachedTrainCar.heal(h);
        }
    }

    public virtual bool destroyLastTrainCar()
    {
        TrainCarController lastTrain = getLastTrain();//.destroyed = true;
        if (lastTrain == null)
            return false;
        lastTrain.destroyed = true;
        return true;
    }

    public TrainCarController getLastTrain()
    {
        TrainCarController lastTrainCar;
        if (attachedTrainCar && !attachedTrainCar.detached)
        {
            lastTrainCar = attachedTrainCar.getLastTrain();
        }
        else
        {
            TrainCarController tc = this as TrainCarController;
            lastTrainCar = tc;
        }
        //print(lastTrainCar.trainCarNumber + " train number");
        return lastTrainCar;
    }

    public void detatchLastTrainCar()
    {
        getLastTrain().Detatch();
        //getLastTrain().attachedTrainCar = null;//the new last train car will have a null attatched train

    }

    public int getTrainsAttatched()
    {
        int trains = 0;
        if (attachedTrainCar)
        {
            trains = attachedTrainCar.getTrainsAttatched();
            trains++;
        }
        return trains;
    }

    public int getHealth()
    {
        int h = 0;
        if (attachedTrainCar)
        {
            h = attachedTrainCar.getHealth();
        }
        h += health;
        return h;
    }

    public int getMaxHealth()
    {
        return getTrainsAttatched() * 50 + 100;
    }

    public void enqueueAttatchedTrain(int turn)
    {
        if (attachedTrainCar)
        {
            attachedTrainCar.enqueueTurn(turn);
        }
    }

    public void straightTrack(float degree)//input the degree of the track and it wil determine if the train will go with or against that degree
    {
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(270, degree + 90, 0);//make train go straight compared to the train track it just collided with
        TurnState = 0;
        degree = (transform.eulerAngles.y) * (Mathf.PI / 180);
    }

    public void TurnRight(Vector3 TurnPoint, float newDegree)//gets the center point the turn will happen on, gets the degree the track is at
    {
        if (TurnState == 1)
            return;
        turnRadius = 20.48f;
        turnAmount = currentSpeed / turnRadius;//pi/2 = 90 degrees, (pi/2)/speed
        turnPoint = TurnPoint;
        TurnState = 1;
        currentDegree = newDegree;
        currentDegree += turnAmount;
    }

    public void TurnLeft(Vector3 TurnPoint, float newDegree)//gets the center point the turn will happen on, gets the degree the track is at
    {
        if (TurnState == -1)
            return;
        turnRadius = 20.48f;
        turnAmount = currentSpeed / turnRadius;
        turnPoint = TurnPoint;
        TurnState = -1;
        currentDegree = newDegree;
        currentDegree -= turnAmount;
    }

    public void Boost()
    {
        int boostCost = 1;
        if (coal >= boostCost)
        {
            //AkCallEvent("Train_Whistle");
            coal -= boostCost;
            currentSpeed += (minSpeed * 0.1f);
            if (currentSpeed >= minSpeed * 3)
            {
                currentSpeed = minSpeed * 3;
            }
            if (brake)//if train was braked start train loop
                ;// AkCallEvent("GAMEPLAY_TRAIN_LOOP");
            brake = false;
        }
    }

    public void Brake(bool b)
    {
        brake = b;
        if (attachedTrainCar)
        {
            attachedTrainCar.Brake(b);
        }
        if (brake)
        {
            AkCallEvent("GAMEPLAY_TRAIN_LOOP_STOP");
            AkCallEvent("Train_Brake");
        }
        else//if train is not braked start loop
            AkCallEvent("GAMEPLAY_TRAIN_LOOP");

    }

    public bool isattached(GameObject trainCar)
    {
        if (attachedTrainCar)
        {
            if (trainCar == attachedTrainCar.gameObject)
            {
                return true;
            }
            else
            {
                return attachedTrainCar.isattached(trainCar);
            }
        }
        else
            return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (HealingStationTrigger(other))
        {

        }
        else if (CoalTrigger(other))
        {

        }
        else if (CoalBoostTrigger(other))
        {

        }
        else if (SataliteTrigger(other))
        {

        }
        else if (PelletTrigger(other))
        {

        }
        else if (AddTrainTrigger(other))
        {

        }
        else if (TrainCarTrigger(other))
        {

        }
        else if (TrainEngineEnemyTrigger(other))
        {

        }
        else if (TrainEngineMainPlayerTrigger(other))
        {

        }
    }


    //OnTriggerEnter///////////////////////////////////////////////////////////////////
    public virtual bool HealingStationTrigger(Collider other)
    {
        return false;
    }

    public virtual bool CoalTrigger(Collider other)
    {
        return false;
    }

    public virtual bool CoalBoostTrigger(Collider other)
    {
        return false;
    }

    public virtual bool SataliteTrigger(Collider other)
    {
        return false;
    }

    public virtual bool PelletTrigger(Collider other)
    {
        return false;
    }

    public virtual bool AddTrainTrigger(Collider other)
    {
        return false;
    }

    public virtual bool TrainCarTrigger(Collider other)
    {
        return false;
    }

    public virtual bool TrainEngineEnemyTrigger(Collider other)
    {
        return false;
    }

    public virtual bool TrainEngineMainPlayerTrigger(Collider other)
    {
        return false;
    }
    ///////////////////////////////////////////////////////////////////////////////////

    void FixedUpdate()
    {
        if (currentSpeed <= 0 || brake)
        {
            //AkSoundEngine.PostEvent("Stop_Sound1", gameObject);
        }
        if (destroyed == true)
        {
            GetComponent<Rigidbody>().AddForce((Physics.gravity * GetComponent<Rigidbody>().mass) * 2);
            if (Mathf.Abs(GetComponent<Rigidbody>().velocity.y) <= 0.01)
            {
                Destroy(gameObject);
            }
            return;
        }
        if (health <= 0)
        {
            health = 0;
            Destroy(new Collider());
        }

        /*if (vulnerable)
        {
            vulnerableTime -= Time.fixedDeltaTime;
            if (vulnerableTime <= 0)
            {
                vulnerable = false;
                GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1));
            }
        }*/

        //camera.transform.localPosition = new Vector3 (camera.transform.localPosition.x, (getTrainsAttatched() + 1) * 20.48f + 30.0f, camera.transform.localPosition.z);
        if (brake)
        {
            currentSpeed -= .01f;
            if (currentSpeed < 0)
            {
                currentSpeed = 0f;
            }
        }
        else
        {
            if (currentSpeed < minSpeed)
                currentSpeed = minSpeed;
        }

        if (attachedTrainCar)
        {
            if (attachedTrainCar.detached == false)
            {
                attachedTrainCar.currentSpeed = currentSpeed;
            }
        }
        //currentSpeed -= .0001f;

        if (TurnState == 0)
        {
            transform.position = new Vector3(transform.position.x + Mathf.Cos(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * currentSpeed, transform.position.y, transform.position.z + Mathf.Sin(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * currentSpeed);
            //rigidbody.velocity = -transform.up * speed;//new Vector3(transform.forward.x,0.0f,transform.forward.z) * speed;
        }
        else if (TurnState == -1)//left
        {
            turnAmount = currentSpeed / turnRadius;
            currentDegree -= currentSpeed / turnRadius;

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(270, currentDegree * (180 / Mathf.PI) + 180, 0);

            transform.position = new Vector3(turnPoint.x + Mathf.Cos((Mathf.PI - currentDegree)) * turnRadius, transform.position.y, turnPoint.z + Mathf.Sin((Mathf.PI - currentDegree)) * turnRadius);
        }
        else if (TurnState == 1)//right
        {
            turnAmount = currentSpeed / turnRadius;
            currentDegree += currentSpeed / turnRadius;

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(270, (currentDegree) * (180 / Mathf.PI) + 180, 0);

            transform.position = new Vector3(turnPoint.x + Mathf.Cos(-currentDegree) * turnRadius, transform.position.y, turnPoint.z + Mathf.Sin(-currentDegree) * turnRadius);

        }
    }
}
