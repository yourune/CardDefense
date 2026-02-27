using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseUtil
{
    private static Camera camera = Camera.main;
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        if (Mouse.current == null) return Vector3.zero;
        
        Plane dragPlane = new (Vector3.forward, new Vector3(0, 0, zValue));
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}