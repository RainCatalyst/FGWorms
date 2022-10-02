using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CollisionChecker : MonoBehaviour
{

    [SerializeField] LayerMask mask;
    [SerializeField] float radius = 0.05f;

    public bool isColliding()
    {
        return Physics.CheckSphere(transform.position, radius, mask);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}