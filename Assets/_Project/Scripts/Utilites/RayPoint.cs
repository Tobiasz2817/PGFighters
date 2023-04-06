using UnityEditor;
using UnityEngine;


[DisallowMultipleComponent]
public class RayPoint : MonoBehaviour
{
    [SerializeField] private bool configureBasedOnScale;
    
    [SerializeField] private float rangeRay;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Transform direction;
    
    public RaycastHit GetRayHit() {
        var configureY = new Vector3(0, transform.localScale.y, 0);
        var myPosition = configureBasedOnScale ? transform.position + configureY : transform.position;
        var directionConfigureY = configureBasedOnScale ? configureY : Vector3.zero;
        var directionVector3 = direction != null ? ((this.direction.position + directionConfigureY) - myPosition).normalized : transform.forward; 
        Ray ray = new Ray(myPosition,directionVector3);

        RaycastHit info;
        if (Physics.Raycast(ray, out info,rangeRay,mask)) {
            return info;
        }

        info.point = ray.GetPoint(rangeRay);
        return info;
    }

#if UNITY_EDITOR
    
    
    private void OnDrawGizmos() {
        var raycastHit = GetRayHit();
        var directionPoint = raycastHit.point;
        var configureY = new Vector3(0, transform.localScale.y, 0);
        var myPosition = configureBasedOnScale ? transform.position + configureY : transform.position;
        Handles.DrawLine(myPosition, directionPoint);
    }
#endif
}
