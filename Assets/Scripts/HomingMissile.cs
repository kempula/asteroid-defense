using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {

    //https://www.youtube.com/watch?v=0v_H3oOR0aU

    public Transform target;
    public Rigidbody2D rb;
    public float speed = 5;
    public float rotateSpeed = 200f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
	}

    public void SetTarget(Transform transform) {
        target = transform;
    }
}
