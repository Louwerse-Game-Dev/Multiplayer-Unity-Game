using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<GameObject> targets;

    public Vector3 offset;

    // Same as update, but happens after the normal update method. Move camera after everything else moved.
    void LateUpdate()
    {
        if (targets.Count == 0) return;

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = newPosition;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1) {
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.center;
    }
}
