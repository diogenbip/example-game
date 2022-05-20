using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    float speed = 10f;
    float score =0f;
    //float Dist_wall;
    //float Dist_score;
    //RaycastHit2D ray;

    private manager manager;

    public NN nN;
    public float fitness { get; private set; }
    private bool alive = true;
    private float spawnTime;
    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;
    public void SetBrain(NN brain)
    {
        this.nN = brain;
    }

    private void UseNN()
    {
        float[] inputs = new float[4];
        
        inputs[0] = transform.position.x;
        inputs[1] = manager.nearestWall.transform.position.y;
        inputs[2] = manager.nearestWall.leftX;
        inputs[3] = manager.nearestWall.rightX;

        var output = nN.FeedForward(inputs);

        if (output[0] < 0)
        {
            Left();
        }else if (output[0] >= 0)
        {
            Right();
        }
    }

    void Update()
    {

        if (alive)
        {
            fitness = Time.deltaTime - spawnTime;
            UseNN();
        }

    }

    private void Awake()
    {

        manager = FindObjectOfType<manager>();
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    private void Right()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void Left()
    {
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            alive = false;
            sprite.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            manager.PlayerLose();
        }

    }




}
