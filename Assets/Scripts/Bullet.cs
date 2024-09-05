using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    public float frozenDelay = 1f;
    public Vector3 direction;
    public float acceleration = 5f;

    float bulletLife = 1f;

    private float timer = 0f;

    private bool isFrozen = true;


    // Start is called before the first frame update
    void Start()
    {
        direction = direction.normalized;
        Invoke("Unfreeze", frozenDelay);
    }

    void Unfreeze(){
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen){
            Move();
        }
    }

    void Move()
    {
        var update = direction * speed * Time.deltaTime;
        transform.Translate(update);
    }
}
