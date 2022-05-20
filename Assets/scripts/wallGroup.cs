using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallGroup : MonoBehaviour
{ 
    public float speed;
    public float leftX;
    public float rightX;

    public GameObject leftCube;
    public GameObject rightCube;

    private manager manager;

    public void Awake()
    {
        manager = FindObjectOfType<manager>();
        leftCube.transform.SetParent(this.transform);
        rightCube.transform.SetParent(this.transform);
    }
    public void Start()
    {

        leftX = this.transform.GetChild(0).gameObject.transform.position.x;
        rightX = this.transform.GetChild(1).gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.RemoveUpWall();
        }

    }

}
