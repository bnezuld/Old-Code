using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

    public GameObject o;
    public int yOffSet = 5;
    private bool RighClick;
    private Vector2 lastMousePoint;
    private float startHeight;

    // Use this for initialization
    void Start () {
        startHeight = transform.position.y;
        //yOffSet = 5;
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
            Vector2 mousePoint = Input.mousePosition;

            float x = Input.GetAxis("Mouse X") ;//mousePoint.x - lastMousePoint.x;
            float y = Input.GetAxis("Mouse Y");// * 100.0f * Time.deltaTime;// mousePoint.y - lastMousePoint.y;//Input.GetAxis("Mouse Y") * 100.0f * Time.deltaTime;
            
            
            Vector3 oPos = o.transform.position;
            oPos.y += yOffSet;

            transform.RotateAround(oPos, transform.up, x * Time.deltaTime * 100);
            transform.RotateAround(oPos, transform.right, -y * Time.deltaTime * 100);

            if (transform.position.y < startHeight)
            {
                transform.RotateAround(oPos, transform.right, y * Time.deltaTime * 100);
            }
           
            transform.LookAt(oPos);
            lastMousePoint = mousePoint;
        }

        float scrollMove = Input.GetAxis("Mouse ScrollWheel");
        if (scrollMove != 0)
        {
            if (scrollMove > 0)
                transform.Translate(transform.forward * 1 * 10, Space.World);//distance its suppose to move = scrollMove * 100
            else
                transform.Translate(transform.forward * -1 * 10, Space.World);//distance its suppose to move = scrollMove * 100

            Vector3 relativePoint = transform.InverseTransformPoint(o.transform.position);
            //print(Vector3.Dot(o.transform.up.normalized, transfor .normalized));
            //print("Scroll " + Vector3.Distance(beforPos, o.transform.position) + " " + Vector3.Distance(transform.position, o.transform.position) + " " + (Vector3.Distance(transform.position, o.transform.position) - Vector3.Distance(beforPos, o.transform.position)) + " " + Vector3.Distance(beforPos, transform.position));//Vector3.Angle(transform.position.normalized, o.transform.position.normalized));
            //Vector3.
            if (transform.position.y < startHeight)//Mathf.Abs(befor - Vector3.Distance(transform.position, o.transform.position)) == Vector3.Distance(beforPos, transform.position))//the object is behind the camera
            {
                //transform.Translate(-transform.forward * scrollMove * 100, Space.World);
                Vector3 oPos = o.transform.position;
                oPos.y = startHeight;
                transform.position = oPos;
                transform.Translate(-transform.forward * 1, Space.World);
            }
            if (Vector3.Distance(transform.position, o.transform.position) > 200)
                transform.Translate(-transform.forward * scrollMove * 100, Space.World);
        }
    }
}
