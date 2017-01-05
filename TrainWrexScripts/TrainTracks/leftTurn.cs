using UnityEngine;
using System.Collections;

public class leftTurn : MonoBehaviour {

    Vector3 leftTurnPoint;

	// Use this for initialization
	void Start () {
        float rotationRadians = (transform.eulerAngles.y * (Mathf.PI / 180));
        leftTurnPoint = new Vector3(transform.position.x + (Mathf.Sin(rotationRadians) * 20.48f) - (Mathf.Cos(rotationRadians) * (transform.localScale.x / 2)),
                                                        transform.position.y,
                                                        transform.position.z + (Mathf.Cos(rotationRadians) * 20.48f) + (Mathf.Sin(rotationRadians) * (transform.localScale.x / 2)));
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrainController>() != null && other.GetType() == typeof(BoxCollider))
        {
            //print(Vector3.Dot(transform.right.normalized, -other.transform.up.normalized) + " " + other.tag);

            if (Vector3.Dot(transform.right.normalized, -other.transform.up.normalized) > 0.5)//facing the same direction
            {
                float degree = (transform.eulerAngles.y) * (Mathf.PI / 180) + (3 * Mathf.PI) / 2;
                other.GetComponent<TrainController>().TurnLeft(leftTurnPoint, degree);
            }
        }
    }
}
