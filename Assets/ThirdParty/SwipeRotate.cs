using UnityEngine;

public class SwipeRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.1f;
    private Vector2 previousMousePosition;
    private Vector2 deltaMousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            deltaMousePosition = (Vector2)Input.mousePosition - previousMousePosition;
            RotateObject(deltaMousePosition);
            previousMousePosition = Input.mousePosition;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                RotateObject(touch.deltaPosition);
            }
        }
    }

    private void RotateObject(Vector2 delta)
    {
        float rotationY = -delta.x * rotationSpeed;
        transform.Rotate(Vector3.up, rotationY, Space.World);
    }
}
