using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera camera;
    GameObject[] players;

    public float curZoomOutScale;

    public int defaultZoomOutScale = 10;

    public int minZoomOutScale = 5;

    public int maxZoomOutScale = 13;

    public Vector3 zoomVector = new Vector3(0, 1, -1f);


    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        curZoomOutScale = defaultZoomOutScale;
        StartCoroutine(CameraOffset());
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 0)
        {
            Vector3 pos = camera.WorldToViewportPoint(FindCenterPoint(players));
            pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
            pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
            //transform.position = Vector3.Slerp(transform.position, camera.ViewportToWorldPoint(pos), Time.deltaTime);
            transform.position = camera.ViewportToWorldPoint(pos);

            camera.transform.localPosition += zoomVector * curZoomOutScale;
        }
    }

    IEnumerator CameraOffset()
    {
        while (true)
        {
            if(players.Length > 0)
            {
                // If players are visible, zoom closer
                if (ObjectsVisible(players))
                {
                    if (curZoomOutScale > minZoomOutScale) curZoomOutScale -= 0.1f;
                }
                else
                {
                    if (curZoomOutScale < maxZoomOutScale) curZoomOutScale += 0.1f;
                }
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool ObjectsVisible(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (!IsTargetVisible(obj))
            {
                return false;
            }
        }
        return true;
    }

    bool IsTargetVisible(GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }

    private bool IsVisible(GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds))
            return true;
        else
            return false;
    }

    Vector3 FindCenterPoint(GameObject[] objects)
    {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0;
        foreach (GameObject obj in objects)
        {
            center += obj.transform.position;
            count++;
        }
        Vector3 centerPosition = center / count;
        return centerPosition;
    }
}
