using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<Mech> _mechs = new List<Mech>();

    public static Map GetMap()
    {
        var map = GameObject.Find("Map");
        if (map != null)
        {
            return map.GetComponent<Map>();
        }

        return null;
    }

    public Mech GetEnemy(int handle)
    {
        foreach(Mech enemy in _mechs) {
            if (enemy.Attack.Handle == handle) {
                return enemy;
            }
        }
        return null;
    }

    public Mech GetNearestEnemy(int handle, Vector3 playerPos, float distMin)
    {
        Mech nearestEnemy = null;

        foreach (Mech enemy in _mechs) {
            if (handle == enemy.Attack.Handle) continue;
            
            var distSqr = GetDistanceSqr(enemy.transform.position, playerPos);
            if (distSqr < distMin) {
                distMin = distSqr;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    float GetDistanceSqr(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
    }
}
