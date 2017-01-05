using UnityEngine;
using System.Collections;

public class MouseRotate : MonoBehaviour {

    bool RighClick;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            RighClick = true;
        }

        if (Input.GetMouseButtonUp(1))

        {
            RighClick = false;
        }
        
        if (RighClick)
        {
            float x = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * 100.0f * Time.deltaTime;
            Vector3 v3 = transform.localEulerAngles;
            //v3.x += x;
            v3.z += y;
            transform.localEulerAngles = v3;
            v3 = new Vector3(0,x,0);
            //transform.Rotate(v3);
            //transform.Rotate(v3);
            v3 = transform.localEulerAngles;
            /*if (transform.localEulerAngles.z > 270)
            {
                v3.z = 270;
            }
            if (transform.localEulerAngles.z < 90)
            {
                v3.z = 90;
            }//*/
            v3.z = Mathf.Clamp(v3.z, 90, 270);
            v3.y = 270;
            transform.localEulerAngles = v3;
        }
    }
}
