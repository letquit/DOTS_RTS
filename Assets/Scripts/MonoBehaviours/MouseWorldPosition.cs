using System;
using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // private void Update()
    // {
    //     Debug.Log(GetPosition());
    // }

    public Vector3 GetPosition()
    {
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // if (Physics.Raycast(mouseCameraRay, out RaycastHit raycastHit, LayerMask.GetMask("Default")))
        // {
        //     return raycastHit.point;
        // }
        
        Plane palne = new Plane(Vector3.up, Vector3.zero);

        if (palne.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        } 
        else
        {
            return Vector3.zero;
        }
    }
}
