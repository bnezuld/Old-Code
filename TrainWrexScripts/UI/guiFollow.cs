using UnityEngine;
using System.Collections;

public class guiFollow : MonoBehaviour {
	
	private float x,y,z;
	private float xRotate,yRotate,zRotate;
	public GameObject follow;
	public GameObject State1;
	public GameObject State2;
	public GameObject State3;
    public bool rotateWith;
	
	private int previousHealth, update;
	private bool damaged;
	// Use this for initialization
	void Start () {
		x = transform.position.x;
		y = transform.position.y;
		z = transform.position.z;
		xRotate = transform.rotation.eulerAngles.x;
		yRotate = 0;
		zRotate = transform.rotation.eulerAngles.z;

        transform.position = new Vector3(x + follow.transform.position.x, y, z + follow.transform.position.z);
        if (rotateWith)
            transform.rotation = Quaternion.Euler(new Vector3(xRotate, follow.transform.rotation.eulerAngles.y + yRotate, zRotate));
        /*State1.GetComponent<Renderer>().enabled = true;
		State2.GetComponent<Renderer>().enabled = false;
		State3.GetComponent<Renderer>().enabled = false;*/
    }

	// Update is called once per frame
	void Update () {
		if (follow) 
		{
			transform.position = new Vector3 (x + follow.transform.position.x, y, z + follow.transform.position.z);
            if(rotateWith)
			    transform.rotation = Quaternion.Euler(new Vector3 (xRotate,follow.transform.rotation.eulerAngles.y + yRotate,zRotate));
		}
		if(follow && (TrainEngineMainPlayer)follow.GetComponent(typeof(TrainEngineMainPlayer)))
		{
            TrainEngineMainPlayer train = (TrainEngineMainPlayer)follow.GetComponent(typeof(TrainEngineMainPlayer));
			int trainHelth = train.getHealth();
			if(trainHelth < previousHealth)
			{
				damaged = true;
				update = 0;
			}
			
			/*if(trainHelth < 50)
			{
				State1.GetComponent<Renderer>().enabled = false;
				State2.GetComponent<Renderer>().enabled = true;
				State3.GetComponent<Renderer>().enabled = false;
			}else if(trainHelth < 25)
			{
				State1.GetComponent<Renderer>().enabled = false;
				State2.GetComponent<Renderer>().enabled = false;
				State3.GetComponent<Renderer>().enabled = true;
			}else
			{
				State1.GetComponent<Renderer>().enabled = true;
				State2.GetComponent<Renderer>().enabled = false;
				State3.GetComponent<Renderer>().enabled = false;
			}*/

			previousHealth = train.getHealth();
			if(damaged == true)
			{
				update++;
				this.transform.localPosition = this.transform.localPosition + Random.insideUnitSphere * 1;

				if(update > 25)
				{
					damaged = false;
				}
			}
			
			/*if(train.reverse())
			{
				yRotate = 180;
			}else
			{
				yRotate = 0;
			}*/
		}
	}
}
