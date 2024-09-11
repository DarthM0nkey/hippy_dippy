using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Lungs")]
    public float airThrust = 1f;
    public float waterThrust = 0.75f;
    public int lungCapacity = 3;
    public float lungCooldown = 0.5f;
    private float lastMoveTime;
    private bool inWater = false;
    private Lungs lungs;

    [Header("Physics")]
    [SerializeField]
    private Rigidbody2D rb2D;

    private bool triggerJump;
    private bool triggerDive;


    void Move(bool moveUp)
    {
        lastMoveTime = Time.time;
        var direction = moveUp ? transform.up : -transform.up;
        LungElements elementToBreath = inWater ? LungElements.Water : LungElements.Air;
        LungElements expelledElement = lungs.TakeBreath(elementToBreath);
        var elementThrust = expelledElement == LungElements.Air ? airThrust : waterThrust;
        rb2D.AddForce(direction * elementThrust, ForceMode2D.Impulse);
    }



    void Start()
    {
        lastMoveTime = Time.time - 0.5f;
        lungs = new Lungs(lungCapacity, LungElements.Air);
    }

    bool CanMove()
    {
        return Time.time - lastMoveTime >= lungCooldown;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove())
        {
            triggerJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove())
        {
            triggerDive = true;
        }
       // else if (Input.touchCount > 0)
        {
            //TODO:
            //If touch is upper half of screen jump
            //Else - dive
           //Touch touch = Input.GetTouch(0);
           // if (touch.phase == TouchPhase.Began)
           // {
           //     triggerJump = true;
           // }
        }
    }

    public void FixedUpdate()
    {
        if (triggerJump)
        {
            Move(moveUp: true);
        }
        else if (triggerDive)
        {
            Move(moveUp: false);
        }
        triggerJump = false;
        triggerDive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

        }
    }
}
