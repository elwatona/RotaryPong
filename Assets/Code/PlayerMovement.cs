using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int playerNum;
    [SerializeField] float playerSpeed;
    [SerializeField] float rotationAmmount;
    [SerializeField] float distFromCenter;
    [SerializeField] bool interpolatedRotation;
    [SerializeField] float interpolatedRotationSpeed;

    private Rigidbody rb;
    private Vector3 startingPoint;
    private bool leftRotInput;
    private bool rightRotInput;
    private float h;
    private float v;
    private float timeOutside;

    private void Start()
    {
        GetPlayerPrefs();
        rb = GetComponent<Rigidbody>();
        startingPoint = transform.position;
    }

    ///<summary> Toma los valores almacenados en PlayerPrefs de cada variable </summary>
    void GetPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("playerSpeed"))
        {
            playerSpeed = PlayerPrefs.GetFloat("playerSpeed");
        }
        if (PlayerPrefs.HasKey("playerSpeed"))
        {
            rotationAmmount = PlayerPrefs.GetFloat("playerRotAmmount");
        }
        if (PlayerPrefs.HasKey("playerRotSmooth"))
        {
            int interpolatedRotBool = PlayerPrefs.GetInt("playerRotSmooth");
            if (interpolatedRotBool == 0)
            {
                interpolatedRotation = false;
            }
            else if (interpolatedRotBool == 1)
            {
                interpolatedRotation = true;
            }
        }
        if (PlayerPrefs.HasKey("playerRotSpeed"))
        {
            interpolatedRotationSpeed = PlayerPrefs.GetFloat("playerRotSpeed");
        }
    }

    private void Update()
    {
        GetInput();
        CheckDistanceFromCenter();
    }

    void CheckDistanceFromCenter()
    {
        float dist = Vector2.Distance(transform.position, Vector2.zero);
        if (dist >= distFromCenter)
        {
            timeOutside += Time.deltaTime;
            if (timeOutside >= 1)
            {
                transform.position = startingPoint;
            }
        }
        else
        {
            timeOutside = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    void Rotation()
    {
        float rotationInZ = transform.eulerAngles.z;

        if (leftRotInput)
        {
            rotationInZ -= rotationAmmount;
        }
        else if (rightRotInput)
        {
            rotationInZ += rotationAmmount;
        }

        if (!interpolatedRotation)
        {
            if (leftRotInput)
            {
                leftRotInput = false;
            }
            if (rightRotInput)
            {
                rightRotInput = false;
            }

            rb.rotation = Quaternion.Euler(0, 0, rotationInZ);
        }
        else
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(0, 0, rotationInZ), Time.deltaTime * interpolatedRotationSpeed);
        }
    }

    private void Movement()
    {
        rb.velocity = Vector3.zero;
        Vector3 toMove = new Vector3(h, v, 0);
        float step = Time.deltaTime * playerSpeed;
        toMove *= step;
        rb.position += toMove;
    }

    void GetInput()
    {
        if (playerNum == 1)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.J))
            {
                leftRotInput = true;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                leftRotInput = false;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                rightRotInput = true;
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                rightRotInput = false;
            }
        }
        else if (playerNum == 2)
        {
            h = Input.GetAxis("Horizontal2");
            v = Input.GetAxis("Vertical2");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                leftRotInput = true;
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                leftRotInput = false;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                rightRotInput = true;
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                rightRotInput = false;
            }
        }
    }

}