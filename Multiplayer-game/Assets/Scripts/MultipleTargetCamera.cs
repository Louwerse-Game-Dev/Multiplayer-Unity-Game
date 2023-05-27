using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<GameObject> targets;

    public Vector3 offset;
    public float smoothTime = .5f;

    public float minZoom = 7f;
    public float maxZoom = 4.6f;
    public float zoomLimiter = 25f;
    public float minDistForZoom = 4f;

    private Bounds bounds;
    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Same as update, but happens after the normal update method. Move camera after everything else moved.
    void LateUpdate()
    {
        if (targets.Count == 0) return;

        bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].transform.position);
        }

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1) {
            return targets[0].transform.position;
        }

        return bounds.center;
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    /*
    * Returns the distance between the 2 players furthest apart.
    */
    float GetGreatestDistance()
    {
        float greatestDistance = Mathf.Max(bounds.size.x, bounds.size.y);

        Debug.Log(greatestDistance);

        if (greatestDistance < minDistForZoom) return minDistForZoom;

        return greatestDistance;
    }
}
