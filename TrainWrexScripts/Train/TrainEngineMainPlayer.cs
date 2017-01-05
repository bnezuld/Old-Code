using UnityEngine;
using System.Collections;

public class TrainEngineMainPlayer : TrainEngine {

    public float mapTime = 15;
    public int boost = 0;
    public int nextTurn = 0;

	// Use this for initialization
	void Start () {
        int trainMesh = 0;
        string zone = PlayerPrefs.GetString("zone");
        if (zone == "mountain")
        {
            trainMesh = 0;
        }

        GameObject tc;
        tc = (GameObject)Resources.Load("trainEngineMainPlayer_" + trainMesh);
        GetComponent<MeshFilter>().mesh = tc.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().material = tc.GetComponent<Renderer>().sharedMaterial;

        if (TrainCarsAttached == 0)
            return;
        attachedTrainCar = (TrainCarController)Instantiate(trainCarCopy, new Vector3(transform.position.x - Mathf.Cos(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * 10.5f, transform.position.y - 0.4f, transform.position.z - Mathf.Sin(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * 10.5f), Quaternion.Euler(270, transform.rotation.eulerAngles.y, 0));
        attachedTrainCar.activate(this.GetComponent<TrainEngine>(), null, TrainCarsAttached);
    }

    public void NextTurnState(int nextTurn)
    {
        ;// this.nextTurn = nextTurn;
    }

    public override bool HealingStationTrigger(Collider other)
    {
        if (other.name.StartsWith("HealingGrid"))
        {
            AkCallEvent("Train_Bell");
            return true;
        }
        return false;
    }

    public override bool CoalTrigger(Collider other)
    {
        if (other.name.StartsWith("P_Coal_Black"))
        {
            Destroy(other.gameObject);
            AkCallEvent("Coal_collect");
            coal++;

            if (gameState == 1)//coal collecting
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + 1);
            return true;
        }
        return false;
    }

    public override bool TrainEngineEnemyTrigger(Collider other)
    {
        if (other.tag == "EnemyPlayer")
        {
            TrainEngineEnemy enemy = (TrainEngineEnemy)other.gameObject.GetComponent(typeof(TrainEngineEnemy));

            if (!enemy.destroyed)
                if (!destroyLastTrainCar())
                    DestroyTrain(other.gameObject);

            enemy.DestroyTrain(gameObject);
            AkCallEvent("");
            if (!destroyed)
                if (gameState == 0 || gameState == 3)//train wreck mode(1) or boss/survival(3)
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + 1);
            return true;
        }
        return false;
    }

    public override bool CoalBoostTrigger(Collider other)
    {
        if (other.name.StartsWith("P_Coal_Boost"))
        {
            Destroy(other.gameObject);
            boost += 1;
            return true;
        }
        return false;
    }
}
