using UnityEngine;
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [Header("Look")]
    [SerializeField] private float minAngle = -30f;
    [SerializeField] private float maxAngle = 40f;
    [SerializeField] private float sensitivity = 0.1f;
    [Header("Cinematic")]
    [SerializeField] private float cinematicMoveDuration = 3f;
    [SerializeField] private float cinematicStayDuration = 2f;
    private float xRotation;
    private Vector2 lookInput;
    private bool isCinematic = false;
    private float cinematicTimer = 0f;
    private Transform cinematicTarget;
    private Vector3 cinematicStartPos;
    private Quaternion cinematicStartRot;
    private Vector3 cinematicTargetPos;
    private Quaternion cinematicTargetRot;
    private void Start()
    {
        LockCursor();
        cinematicTarget = GameManager.Instance.door.transform;
    }
    public void SetLookInput(Vector2 input)
    {
        if (isCinematic) return;
        lookInput = input;
    }
    public void Look()
    {
        if (isCinematic)
        {
            PlayCinematic();
            return;
        }
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);
        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    public void StartCinematic()
    {
        if (cinematicTarget == null) return;
        isCinematic = true;
        cinematicTimer = 0f;
        cinematicStartPos = cameraTransform.position;
        cinematicStartRot = cameraTransform.rotation;
        cinematicTargetPos = cinematicTarget.position + new Vector3(-2f, 2f, -5f);
        cinematicTargetRot = Quaternion.Euler(0f, 0f, 0f);
        Time.timeScale = 0f;
    }
    private void PlayCinematic()
    {
        cinematicTimer += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(cinematicTimer / cinematicMoveDuration);
        float smoothT = Mathf.SmoothStep(0f, 1f, t);
        cameraTransform.position = Vector3.Lerp(cinematicStartPos, cinematicTargetPos, smoothT);
        cameraTransform.rotation = Quaternion.Slerp(cinematicStartRot, cinematicTargetRot, smoothT);
        if (cinematicTimer >= cinematicMoveDuration + cinematicStayDuration)
            EndCinematic();
    }
    private void EndCinematic()
    {
        Time.timeScale = 1f;
        isCinematic = false;
        cinematicTimer = 0f;
        cameraTransform.localPosition = new Vector3(0, 0.5f, 0.2f);
        xRotation = cameraTransform.localEulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f;
    }
    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}