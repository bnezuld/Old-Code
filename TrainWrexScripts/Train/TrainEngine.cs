using UnityEngine;
using System.Collections;

public class TrainEngine : TrainController {

    public int TrainCarsAttached;
    public TrainCarController trainCarCopy;//contains the train car that will be used

    // Use this for initialization
    void Start () {
        if (attachedTrainCar)
            trainCarCopy = attachedTrainCar;
        Vector3 pos = transform.position;
        pos += transform.right * 10.5f;
        attachedTrainCar = (TrainCarController)Instantiate(this, pos, Quaternion.Euler(270, transform.rotation.eulerAngles.y, 0)); attachedTrainCar.activate(this.GetComponent<TrainEngine>(), null, TrainCarsAttached);
    }

    public void addOneTrainCar()
    {
        if (attachedTrainCar)
        {
            attachedTrainCar.addOneTrainCar(trainCarCopy);
        }
        else if (trainCarCopy)
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
            attachedTrainCar.activate(this.GetComponent<TrainEngine>(), null, 1);
        }
    }

    public override bool CoalTrigger(Collider other)
    {
        if (gameState != 1/*coal collect*/ && other.name.StartsWith("P_Coal_Black"))
        {
            Destroy(other.gameObject);
            coal++;
            return true;
        }
        return false;
    }

    public override bool SataliteTrigger(Collider other)
    {
        if (other.name.StartsWith("HealingGrid"))
        {
            Destroy(other.gameObject);
            return true;
        }
        return false;
    }

    public override bool PelletTrigger(Collider other)
    {
        if (other.name.StartsWith("HealingGrid"))
        {
            Destroy(other.gameObject);
            return true;
        }
        return false;
    }

    public override bool AddTrainTrigger(Collider other)
    {
        if (other.name.StartsWith("P_Add_Train"))
        {
            Destroy(other.gameObject);
            addOneTrainCar();
            return true;
        }
        return false;
    }

    public override bool TrainCarTrigger(Collider other)
    {
        if (other.name.StartsWith("P_TrainCar"))
        {
            TrainCarController enemy = (TrainCarController)other.gameObject.GetComponent(typeof(TrainCarController));
            if (!isattached(other.gameObject) && !this.destroyed)
                enemy.DestroyTrain(other.gameObject);
            return true;
        }
        return false;
    }

    public override bool TrainEngineMainPlayerTrigger(Collider other)
    {
        if (other.name.StartsWith("P_Train_Main"))
        {
            return true;
        }
        return false;
    }
}
