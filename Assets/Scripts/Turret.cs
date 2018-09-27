using UnityEngine;

public class Turret : MonoBehaviour {

    public float missileSpeed = 5f;
    public GameObject missileGO;
    public GameObject homingMissileGO;

    public float trajectoryLength = 50f;
    bool isUsingHomingMissile;

    public void OnAsteroidSpawned(Vector3 asteroidPosition, Vector3 asteroidVelocity, Transform target) {
        if(TrajectoryWithinSafetyZone(asteroidPosition, asteroidVelocity)) {

            if(isUsingHomingMissile) {
                ShootHomingMissile(target);
                return;
            }

            Vector3 velocity = CalculateMissileVelocity(asteroidPosition, asteroidVelocity);

            //Okay, about the next rotation thingy I'm not so sure of, but it works... :D
            var relativePos = target.position - transform.position;
            var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            angle = angle - 90f;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            ShootStandardMissile(velocity, rotation);
        }
    }

    private Vector3 CalculateMissileVelocity(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
        return FirstOrderIntercept(transform.position, Vector3.zero, missileSpeed, asteroidPosition, asteroidVelocity);
    }

    private void ShootStandardMissile(Vector3 velocity, Quaternion rotation) {;
        GameObject go = (GameObject)Instantiate(missileGO, transform.position, rotation);
        Missile missile = go.GetComponent<Missile>();
        missile.SetVelocity(velocity);
    }

    void ShootHomingMissile(Transform target) {
        GameObject go = (GameObject)Instantiate(homingMissileGO, transform.position, Quaternion.identity);
        HomingMissile homingMissile = go.GetComponent<HomingMissile>();
        homingMissile.SetTarget(target);
    }

    private bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
        int layerMask = LayerMask.GetMask("Raycast");
        if(Physics2D.Raycast(asteroidPosition, asteroidVelocity, trajectoryLength, layerMask)) {
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

    //http://wiki.unity3d.com/index.php/Calculating_Lead_For_Projectiles
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
