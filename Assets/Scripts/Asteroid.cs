using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public GameObject turretGameObject;
    public GameObject asteroid;
    public GameObject explosion;

    public Rigidbody2D rb;
    private float asteroidSpeed = 3.0f;
    private Vector3 velocity;

    public Turret turret;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: DELETE ASTEROID WHEN OF SCREEN
	}

    private void FixedUpdate() {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Missile") {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    public void SetVelocity(Vector3 vector3) {
        velocity = vector3 * asteroidSpeed;
        turretGameObject = GameObject.FindGameObjectWithTag("turret");
        turret = turretGameObject.GetComponent<Turret>();
        Vector3 asteroidPosition = asteroid.transform.position;
        Vector3 asteroidVelocity = velocity;
        turret.OnAsteroidSpawned(asteroidPosition, asteroidVelocity, rb, transform);
    }


}
