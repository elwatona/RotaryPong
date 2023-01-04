using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallPaint
{
    noPaint,
    pink,
    blue
}

public class Ball : MonoBehaviour
{
    public BallPaint paint;
    public Material[] playerMaterials;
    public bool canScore;
    [SerializeField]
    float ballSpeed;
    [SerializeField]
    float distFromCenter;
    [SerializeField]
    float minVelocity;
    [SerializeField]
    float colorDuration;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    AudioClip[] playerAudioClips;

    AudioSource aud;
    float colorTimer;
    float timeOutside;
    Material startingMaterial;
    Color startingColor;
    Vector3 lastFrameVelocity;
    Vector3 startingPoint;
    Renderer rend;
    Collider coll;
    TrailRenderer trailRend;

    private void Start()
    {
        if (PlayerPrefs.HasKey("ballSpeed"))
        {
            ballSpeed = PlayerPrefs.GetFloat("ballSpeed");
        }
        if (PlayerPrefs.HasKey("ballDrag"))
        {
            rb.drag = PlayerPrefs.GetFloat("ballDrag");
        }
        if (PlayerPrefs.HasKey("ballMinSpeed"))
        {
            minVelocity = PlayerPrefs.GetFloat("ballMinSpeed");
        }
        if (PlayerPrefs.HasKey("ballColorDuration"))
        {
            colorDuration = PlayerPrefs.GetFloat("ballColorDuration");
        }
        aud = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
        trailRend = GetComponent<TrailRenderer>();
        startingMaterial = rend.material;
        startingColor = trailRend.startColor;
        startingPoint = transform.position;
        canScore = true;
    }

    public void TurnBackOn()
    {
        transform.position = startingPoint;
        rb.velocity = Vector3.zero;
        rend.enabled = true;
        coll.enabled = true;
        trailRend.enabled = true;
        canScore = true;
    }

    public void SetInvis()
    {
        LosePaint();
        rend.enabled = false;
        coll.enabled = false;
        trailRend.enabled = false;
        canScore = false;
    }

    private void Bounce(Vector3 collisionNormal)
    {
        float speed = lastFrameVelocity.magnitude;
        if (speed <= minVelocity)
        {
            rb.velocity = collisionNormal * ballSpeed;
        }
        else
        {
            Vector3 ballVelocity = lastFrameVelocity.normalized;
            Vector3 direction = Vector3.Reflect(ballVelocity, collisionNormal);
            if (speed <= ballSpeed)
            {
                speed = ballSpeed;
            }
            rb.velocity = direction * Mathf.Max(speed, minVelocity);
            Debug.DrawRay(transform.position, direction * 3, Color.red, 5);
        }
    }

    void CheckForPlayer(GameObject collObject)
    {
        if (collObject.tag == "Player")
        {
            if (collObject.name == "p1")
            {
                PlayAudio(0);
                ColorChange(0);
            }
            else if (collObject.name == "p2")
            {
                PlayAudio(1);
                ColorChange(1);
            }
        }
    }

    void PlayAudio(int who)
    {
        aud.clip = playerAudioClips[who];
        aud.Play();
    }

    void ColorChange(int who)
    {
        colorTimer = colorDuration;
        if (who == 0)
        {
            paint = BallPaint.pink;
        }
        else if (who == 1)
        {
            paint = BallPaint.blue;
        }
        rend.material = playerMaterials[who];
        trailRend.material = rend.material;
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;
        CheckPaint();
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

    void CheckPaint()
    {
        if (paint != BallPaint.noPaint)
        {
            colorTimer -= Time.deltaTime;
            if (colorTimer <= 0)
            {
                LosePaint();
            }
        }
    }

    void LosePaint()
    {
        Debug.Log("lose paint");
        paint = BallPaint.noPaint;
        rend.material = startingMaterial;
        //trailRend.startColor = startingColor;
        //trailRend.endColor = startingColor;
        rend.material.SetColor("_EmissionColor", rend.material.color);
        trailRend.material = rend.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
        CheckForPlayer(collision.gameObject);
    }
    
}
