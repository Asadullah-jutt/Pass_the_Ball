using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    [SerializeField] GameObject[] cubearr;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject afterlvl10;
    [SerializeField] GameObject afterlvl15;
    public Color color;
    public Material oldMaterial;
    public Material newMaterial;
    int bcount;
    Vector3 startpos;

    [SerializeField] BoxCollider[] box;

    [SerializeField] TextMeshProUGUI bounceCount ;
    [SerializeField] TextMeshProUGUI lvl;
    Renderer rendererr;
    GameObject turnred;
    private Rigidbody rb;
    bool ingame = false;
    int levelno = 0;
    [SerializeField] private float forceMultiplier = 2f;
    Camera Cam;
    Vector3 lastvelo;
    private bool firstTouchIgnored = false; 
    Vector2 force;
    Vector3 nstartpos, nendpos;
    public Vector2 minpower, maxpower;
  //  int waittime = 60;
    private LineRenderer line;// line2;
    int touchcount = 1;
    Touch touch;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        line = GetComponent<LineRenderer>();
        //line2 = new LineRenderer();
     //   line2.enabled = false;
        line.enabled = false;
        Cam = Camera.main;
        startpos = new Vector3 (ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);
        
        lvl.text = "LEVEL " + levelno.ToString();
        bcount = 5;
        rb = ball.GetComponent<Rigidbody>();
        NextLevel();
       
        ingame = false;
        bounceCount.text = bcount.ToString();
    }
    void NextLevel()
    {
        levelno++;
        boxstate(true,levelno);
        ingame = true;
        rb.velocity = Vector3.zero;
        bcount = bouncesetter(levelno);
        bounceCount.text = bcount.ToString();
     
        lvl.text = "LEVEL " + levelno.ToString();
        ball.transform.position = startpos;
        rb.velocity = Vector3.zero;
        BoxCollider a;
       
        if (turnred != null)
        {
            a = turnred.GetComponent<BoxCollider>();
            Destroy(a);
        }
        if (rendererr != null)
        {
            rendererr.material = oldMaterial;
        }

        turnred = cubearr[Random.Range(0, cubearr.Length)];
        a = turnred.AddComponent<BoxCollider>();
        rendererr = turnred.GetComponent<Renderer>();
        if (rendererr != null)
        {
            a.isTrigger = true;
            // Remove the previous material
            // Add the new material
            rendererr.material = newMaterial;
        }

    }
    int bouncesetter(int lvl)
    {
        if (lvl <= 3)
            return lvl;
        else if(lvl > 3 && lvl < 10 )
        {
           return Random.Range(2, 6);
        }
        else
        {
            return Random.Range(4, 10);
        }

    }

    void Update()
    {
       
        if (ingame)
        {
            if (bcount < 0)
            {
                Debug.Log("Failed");
                bounceCount.enabled = false;
                lvl.enabled = false;
                PlayerPrefs.SetInt("yourscore", levelno);
                PQhandler.instance.CheckScore();
            }
        }
        // Touch input 
        if (Input.touchCount > 0)
        {

          
            touch = Input.GetTouch(0);
           

            if (touch.phase == TouchPhase.Began )
            {
                
                line.enabled = true;
                //  line2.enabled = true;
                nstartpos = Cam.ScreenToWorldPoint(Input.mousePosition);
                nstartpos.z = 0f;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                nendpos = Cam.ScreenToWorldPoint(Input.mousePosition);
                nendpos.z = 0f;
                startpos.z = 0f;
                force = new Vector2(Mathf.Clamp((startpos.x - nendpos.x), minpower.x, maxpower.x), Mathf.Clamp((startpos.y - nendpos.y), minpower.y, maxpower.y));
                Vector2 endPos = (Vector2)startpos + force;
                RenderLine(startpos, endPos);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                nendpos = Cam.ScreenToWorldPoint(Input.mousePosition);
                nendpos.z = 0f;
                startpos.z = 0f;
                // rb.useGravity = true;
                touchcount--;
                if (touchcount > 0)
                    return;
                force = new Vector2(Mathf.Clamp((startpos.x - nendpos.x), minpower.x, maxpower.x), Mathf.Clamp((startpos.y - nendpos.y), minpower.y, maxpower.y));
                rb.AddForce(force * forceMultiplier, ForceMode.Impulse);
                ingame = true;
                line.enabled = false;
            }
        }
        // Mouse input
        if (Input.GetMouseButtonDown(0))
        {
            line.enabled = true;
            //  line2.enabled = true;
            nstartpos = Cam.ScreenToWorldPoint(Input.mousePosition);
            nstartpos.z = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            nendpos = Cam.ScreenToWorldPoint(Input.mousePosition);
            nendpos.z = 0f;
            startpos.z = 0f;
            force = new Vector2(Mathf.Clamp((startpos.x - nendpos.x), minpower.x, maxpower.x), Mathf.Clamp((startpos.y - nendpos.y), minpower.y, maxpower.y));
            Vector2 endPos = (Vector2)startpos + force;
            RenderLine(startpos, endPos);
            //   Debug.Log("in");
        }

        if (Input.GetMouseButtonUp(0))
        {

            nendpos = Cam.ScreenToWorldPoint(Input.mousePosition);
            nendpos.z = 0f;
            startpos.z = 0f;
            // rb.useGravity = true;
            Debug.Log(touchcount);
            touchcount--;
            //if (touchcount >= 0)
            //    return;
            force = new Vector2(Mathf.Clamp((startpos.x - nendpos.x), minpower.x, maxpower.x), Mathf.Clamp((startpos.y - nendpos.y), minpower.y, maxpower.y));
            rb.AddForce(force * forceMultiplier, ForceMode.Impulse);
            ingame = true;
            line.enabled = false;
            // DrawForceDirection(transform.position, force);
        }



        lastvelo = rb.velocity;
    }
   

   
    private void OnCollisionEnter(Collision collision)
    {
        float speed = lastvelo.magnitude;

        Vector3 dir = Vector3.Reflect(lastvelo.normalized, collision.contacts[0].normal);

        rb.velocity = dir * Mathf.Max(speed, 0f);
        bcount--;
        if(bcount >= 0)
             bounceCount.text = bcount.ToString();
    }
    //  [System.Obsolete]
    // [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
      
        Debug.Log("kmdlksl");
        if (bcount == 0)
        {
            ingame = false;
            boxstate(false,levelno);
            Debug.Log("level Completed - next lvl");
             Invoke("NextLevel", 0.5f);
          //  NextLevel();
          //  rb.velocity = Vector3.zero;
        }
        else if (bcount != 0)
        {
            Debug.Log("NOOOOOOOOOOOOOOOOOOOOO");
            bounceCount.enabled = false;
            lvl.enabled = false;
            PQhandler.instance.CheckScore();
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //}


    void RenderLine(Vector3 spos, Vector3 epos)
    {
      //  line.transform.rotation = Quaternion.identity;
        line.positionCount = 2;
        Vector3[] point = new Vector3[2];
    
        point[0] = spos;
        point[1] = epos;

        line.SetPositions(point);
    }


    void boxstate(bool state, int lvl)
    {
        for (int i = 0; i < box.Length; i++)
        {
            box[i].enabled = state;
        }
        if(lvl >= 5)
        {
            afterlvl10.SetActive(state);
            if (lvl >= 10)
                afterlvl15.SetActive(state);
        }
    }
  




}
