using UnityEngine;
using System.Collections;

public class CameraMoverController : MonoBehaviour {

    private bool rightClick;
    public float max = 300;
    public float min = 25;
    public float scrollSpeed = 10;
    private int range = 25;
    public Transform lookAt;

	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollMove = Input.GetAxis("Mouse ScrollWheel");
        if (scrollMove != 0)
        {
            
            transform.Translate(new Vector3(-0.25f,-0.75f, 0) * scrollMove * scrollSpeed,Space.World);
            //Vector3 pos = transform.position;
            if (transform.position.y < min)
            {
                transform.Translate(-new Vector3(-0.25f, -0.75f, 0) * scrollMove * scrollSpeed, Space.World);
            }

            if (transform.position.y > max)
            {
                transform.Translate(-new Vector3(-0.25f, -0.75f, 0) * scrollMove * scrollSpeed, Space.World);
            }
            //transform.position = pos;
            transform.LookAt(lookAt);
            /*Vector3 rotation = transform.localEulerAngles;
            //float a = ((pos.y - min) * (pos.y - min)) / ((max - min) * (max - min))
            print((((pos.y - min) * (pos.y - min)) / ((max - min) * (max - min)) * 45 + 45));
            if (pos.y < min + range)
                rotation.x = (((pos.y - min) * (pos.y - min)) / ((range) * (range)) * 45 + 45);
            else
                rotation.x = 90;
            pos.x = pos.x + (rotation.x - 90)/45 * scrollMove;
            transform.localEulerAngles = rotation;
            transform.position = pos;*/

        }
    }
}
