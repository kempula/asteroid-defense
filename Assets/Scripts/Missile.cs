using UnityEngine;

public class Missile : MonoBehaviour {

    public Rigidbody2D rb;
    public GameObject missile;

    public float missileSpeed = 5f;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        rb.velocity = velocity * missileSpeed;
    }

    public void SetVelocity(Vector3 v) {
        v.Normalize();
        velocity = v;
    }
}
