using UnityEngine;
using System.Collections;

public class TrainEngineEnemy : TrainEngine
{

    // Use this for initialization
    void Start () {
        int trainMesh = 0;
        string zone = PlayerPrefs.GetString("zone");
        if (zone == "mountain")
        {
           trainMesh = 0;
        }


        GameObject tc;
        tc = (GameObject)Resources.Load("trainEngineEnemy_" + trainMesh);
        GetComponent<MeshFilter>().mesh = tc.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().material = tc.GetComponent<Renderer>().sharedMaterial;

        attachedTrainCar = (TrainCarController)Instantiate(trainCarCopy, new Vector3(transform.position.x - Mathf.Cos(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * 10.5f, transform.position.y - 0.4f, transform.position.z - Mathf.Sin(-(transform.eulerAngles.y - 90) * (Mathf.PI / 180)) * 10.5f), Quaternion.Euler(270, transform.rotation.eulerAngles.y, 0));
        attachedTrainCar.activate(this.GetComponent<TrainEngine>(), null, TrainCarsAttached);
    }

    public override bool TrainEngineEnemyTrigger(Collider other)
    {
        if (other.tag == "EnemyPlayer")
        {
            TrainEngineEnemy enemy = (TrainEngineEnemy)other.gameObject.GetComponent(typeof(TrainEngineEnemy));
            if (!enemy.destroyed && !this.destroyed)
                if (gameState == 0 || gameState == 3)//train wreck mode(1) or boss/survival(3)
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + 2);//add 2 for both trains being destroyed
            enemy.DestroyTrain(gameObject);
            Destroy(enemy.gameObject);
            return true;
        }
        return false;
    }
}
