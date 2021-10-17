using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Camera Settings")] [SerializeField]
    private Transform cameraHolder;

    public float camClampXMax = 90f;
    public float camClampXMin = -90f;

    public float mouseSensitivityX;
    public float mouseSensitivityY;
    public bool invertMouseX;
    public bool invertMouseY;
    private Vector3 _cameraRotation;
    private Vector3 _characterRotation;
    private InputMaster _controls;
    private float _mouseX;
    private float _mouseY;
    public Orientation OrientationToSend;

    private void Awake()
    {
        _controls = InputMaster.GetInstance();
        _controls.GroundMovement.MouseX.performed += ctx => _mouseX = ctx.ReadValue<float>();
        _controls.GroundMovement.MouseY.performed += ctx => _mouseY = ctx.ReadValue<float>();
        _cameraRotation = cameraHolder.localRotation.eulerAngles;
        _characterRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        CalculateMouseLook();
    }

    private void CalculateMouseLook()
    {
        _cameraRotation.x += mouseSensitivityY * (invertMouseY ? _mouseY : -_mouseY) * Time.deltaTime;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, camClampXMin, camClampXMax);
        _characterRotation.y += mouseSensitivityX * (invertMouseX ? -_mouseX : _mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(_characterRotation);
        cameraHolder.localRotation = Quaternion.Euler(_cameraRotation);
        OrientationToSend.CameraRotationX = _cameraRotation.x;
        OrientationToSend.CharacterRotationY = _characterRotation.y;
    }
}