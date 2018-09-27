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

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height || screenPosition.y < 0) {
            Destroy(gameObject);
        }

        if(screenPosition.x > Screen.width || screenPosition.x < 0) {
            Destroy(gameObject);
        }
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
    }

    public void OnAsteroidSpawned() {
        turretGameObject = GameObject.FindGameObjectWithTag("Turret");
        turret = turretGameObject.GetComponent<Turret>();
        Vector3 asteroidPosition = asteroid.transform.position;
        Vector3 asteroidVelocity = velocity;
        turret.OnAsteroidSpawned(asteroidPosition, asteroidVelocity, transform);
    }
   
}
