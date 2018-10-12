using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float gravity =5;
    public int moveForce= 5;

    private bool collided;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!collided)
        {
            transform.Translate(Vector3.down * gravity * Time.deltaTime);
            gravity += 0.1f;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * moveForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        collided = true;
    }
    private void OnTriggerExit(Collider other)
    {
        collided = false;
    }
    
}
