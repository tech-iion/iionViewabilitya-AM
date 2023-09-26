using UnityEngine;

public class ObjectVisibilityChecker : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }



    }
    private void Update()
    {
        
    Debug.Log(VisibilityChecker.IsObjectVisible(this.gameObject, mainCamera));
    }

}


public static class VisibilityChecker
{
    public static bool IsObjectVisible(GameObject targetObject, Camera camera)
    {
        Bounds objectBounds = CalculateObjectBounds(targetObject);

        bool isCompletelyVisible = IsObjectCompletelyVisible(objectBounds, camera);
        bool isObstructed = IsObjectObstructed(objectBounds, camera , targetObject);

        //LogDebugInfo(isCompletelyVisible, isObstructed);

        return isCompletelyVisible && !isObstructed;
    }

    private static bool IsObjectCompletelyVisible(Bounds bounds, Camera camera)
    {
        bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bounds);
        return isVisible;
    }

    private static bool IsObjectObstructed(Bounds bounds, Camera camera , GameObject targetObject)
    {
        RaycastHit[] hits = Physics.RaycastAll(camera.transform.position, bounds.center - camera.transform.position, Vector3.Distance(bounds.center, camera.transform.position));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != targetObject)
            {
                //Renderer renderer = hit.collider.GetComponent<Renderer>();
                //if (renderer != null && IsBoundsOverlap(renderer.bounds, bounds))
                //{
                    return true;
               // }
            }
        }

        return false;
    }

    private static bool IsBoundsOverlap(Bounds bounds1, Bounds bounds2)
    {
        return bounds1.Intersects(bounds2);
    }

    private static Bounds CalculateObjectBounds(GameObject targetObject)
    {
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
            return renderer.bounds;

        Collider collider = targetObject.GetComponent<Collider>();
        if (collider != null)
            return collider.bounds;

        Debug.LogError("The target object does not have a Renderer or Collider component.");
        return new Bounds();
    }

    private static void LogDebugInfo(bool isCompletelyVisible, bool isObstructed)
    {
        string visibilityStatus = isCompletelyVisible ? "completely visible" : "not completely visible";
        string obstructionStatus = isObstructed ? "obstructed" : "not obstructed";

        Debug.Log($"Object is {visibilityStatus} and {obstructionStatus}.");
    }
}

