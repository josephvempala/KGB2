using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Camera Settings")] [SerializeField]
    private Transform cameraHolder;

    public float camClampXMax = 90f;
    public float camClampXMin = -90f;

    public float MouseSensitivityX;
    public float MouseSensitivityY;
    public bool InvertMouseX;
    public bool InvertMouseY;
    private Vector3 CameraRotation;
    private Vector3 CharacterRotation;
    private InputMaster controls;
    private float mouseX;
    private float mouseY;
    public Orientation orientationToSend;

    private void Awake()
    {
        controls = InputMaster.GetInstance();
        controls.GroundMovement.MouseX.performed += ctx => mouseX = ctx.ReadValue<float>();
        controls.GroundMovement.MouseY.performed += ctx => mouseY = ctx.ReadValue<float>();
        CameraRotation = cameraHolder.localRotation.eulerAngles;
        CharacterRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        CalculateMouseLook();
    }

    private void CalculateMouseLook()
    {
        CameraRotation.x += MouseSensitivityY * (InvertMouseY ? mouseY : -mouseY) * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, camClampXMin, camClampXMax);
        CharacterRotation.y += MouseSensitivityX * (InvertMouseX ? -mouseX : mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(CharacterRotation);
        cameraHolder.localRotation = Quaternion.Euler(CameraRotation);
        orientationToSend.CameraRotationX = CameraRotation.x;
        orientationToSend.CharacterRotationY = CharacterRotation.y;
    }
}