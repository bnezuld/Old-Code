using UnityEngine;
using System.Collections;

public class RighClickDrag : MonoBehaviour {

    private bool rightClick;
    private bool spaceBar;
    private bool q;
    private bool e;

    public float dragSpeed = 2.5f;
    public float max = 300;
    public float min = 5;
    public float scrollSpeed = 100;
    private int range = 25;
    private int rotationY = 0;
    private int cameraRotationY = 90;

    public Transform gameCamera;
    public Transform player;

	// Use this for initialization
	void Start () {
        gameCamera.LookAt(this.transform);
        /*Vector3 pos = transform.position;
        pos.y = min;
        transform.position = pos;*/
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            rightClick = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rightClick = false;
        }
        if (rightClick)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector3 mouseDrag = new Vector3(-y,0,x);
            mouseDrag *= dragSpeed;
            transform.Translate(mouseDrag);

            //transform.Translate(transform.forward * x * dragSpeed);
            //transform.Translate(-transform.right * y * dragSpeed);
            /*Vector3 pos = transform.position;
            pos.x -= y * dragSpeed;
            pos.z += x * dragSpeed;
            transform.position = pos;//*/
        }

        ///////////////////////////////////////////////////////////

        float scrollMove = Input.GetAxis("Mouse ScrollWheel");
        if (scrollMove != 0)
        {
            gameCamera.LookAt(this.transform);
            Vector3 move = gameCamera.forward * scrollMove * scrollSpeed;
            gameCamera.Translate(move, Space.World);

            /*Vector3 move = new Vector3(0,-1f, 0);
            move *= scrollMove * scrollSpeed;
            gameCamera.Translate(move.x, move.y, move.z, Space.World);
            Vector3 pos = gameCamera.localPosition;
            if (pos.y < min)
            {
                pos.y = min;
            }
            if(pos.y > max)
            {
                pos.y = max;
            }
            gameCamera.localPosition = pos;

            if (gameCamera.localPosition.y < min + range)
            {
                pos = gameCamera.localPosition;
                pos.x = ((pos.y * 0.5f) - (min + range) * 0.5f);
                gameCamera.localPosition = pos;
                gameCamera.LookAt(gameObject.transform);

                Vector3 r = gameCamera.localEulerAngles;
                r.y = cameraRotationY;
                gameCamera.localEulerAngles = r;
            }
            else
            {
                pos.x = 0;
                gameCamera.localPosition = pos;

                Vector3 r = gameCamera.localEulerAngles;
                r.x = 90;
                gameCamera.localEulerAngles = r;
            }*/
        }

        ////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            spaceBar = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            spaceBar = false;
        }

        if (spaceBar)//Input.GetKeyDown("space"))
        { 
            Vector3 pos = player.position;
            pos.y = transform.position.y;
            transform.position = pos;
            print("space");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            q = true;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            q = false;
        }

        if (q)
            rotationY--;

        if (Input.GetKeyDown(KeyCode.E))
        {
            e = true;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            e = false;
        }

        if (e)
            rotationY++;

        Vector3 rotation = transform.localEulerAngles;
        rotation.y = rotationY;
        transform.localEulerAngles = rotation;
    }
}
