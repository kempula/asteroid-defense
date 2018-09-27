using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public float missileSpeed = 5f;
    [SerializeField]
    public GameObject missile;
    public GameObject homingMissile;

    public float trajectoryLenght = 50f;
    bool isUsingHomingMissile;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAsteroidSpawned(Vector3 asteroidPosition, Vector3 asteroidVelocity, Rigidbody2D rb, Transform target) {
        if(TrajectoryWithinSafetyZone(asteroidPosition, asteroidVelocity)) {

            if(isUsingHomingMissile) {
                ShootHomingMissile(target);
                return;
            }

            Vector3 velocity = CalculateMissileVelocity(asteroidPosition, asteroidVelocity);
            Shoot(velocity);
        }
    }

    private Vector3 CalculateMissileVelocity(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
        return FirstOrderIntercept(transform.position, Vector3.zero, missileSpeed, asteroidPosition, asteroidVelocity);
    }

    private void Shoot(Vector3 velocity) {
        GameObject go = (GameObject)Instantiate(missile, transform.position, Quaternion.identity);
        Missile missileScript = go.GetComponent<Missile>();
        missileScript.SetVelocity(velocity);
    }

    void ShootHomingMissile(Transform target) {
        GameObject go = (GameObject)Instantiate(homingMissile, transform.position, Quaternion.identity);
        HomingMissile missileScript = go.GetComponent<HomingMissile>();
        missileScript.SetTarget(target);
    }

    private bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
        int layerMask = LayerMask.GetMask("Raycast");
        if(Physics2D.Raycast(asteroidPosition, asteroidVelocity, trajectoryLenght, layerMask)) {
            return true;
        }

        return false;
    }

    public void ChangeMissile() {
        if(isUsingHomingMissile == true) {
            isUsingHomingMissile = false;
            return;
        }
        isUsingHomingMissile = true;
    }

    //first-order intercept using absolute target position
    public static Vector3 FirstOrderIntercept(
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }
}
