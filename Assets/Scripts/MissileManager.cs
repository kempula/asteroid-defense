using UnityEngine;

public class MissileManager : MonoBehaviour
{

    public void ChangeMissile()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Turret");
        p.GetComponent<Turret>().ChangeMissile();
    }
}
