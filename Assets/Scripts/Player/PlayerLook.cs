using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Header("Look")]
    [SerializeField] private float minAngle = -30f;
    [SerializeField] private float maxAngle = 40f;
    [SerializeField] private float sensitivity = 0.1f;

    private float xRotation;
    private Vector2 lookInput;

    private void Start()
    {
        LockCursor();
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}