using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{

    public void ChangeMissile()
    {
        GameObject p = GameObject.FindGameObjectWithTag("turret");
        p.GetComponent<Turret>().ChangeMissile();
    }
}
