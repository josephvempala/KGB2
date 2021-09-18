using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    private InputMaster controls;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraHolder;
    public float camClampXMax = 90f;
    public float camClampXMin = -90f;
    private float mouseX;
    private float mouseY;
    private Vector3 CameraRotation;
    private Vector3 CharacterRotation;
    public float MouseSensitivityX;
    public float MouseSensitivityY;
    public bool InvertMouseX;
    public bool InvertMouseY;

    void Awake()
    {
        controls = InputMaster.GetInstance();
        controls.GroundMovement.MouseX.performed += ctx => mouseX = ctx.ReadValue<float>();
        controls.GroundMovement.MouseY.performed += ctx => mouseY = ctx.ReadValue<float>();
        CameraRotation = cameraHolder.localRotation.eulerAngles;
        CharacterRotation = transform.localRotation.eulerAngles;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMouseLook();
    }

    public void CalculateMouseLook()
    {
        CameraRotation.x += MouseSensitivityY * (InvertMouseY ? mouseY : -mouseY) * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, camClampXMin, camClampXMax);
        CharacterRotation.y += MouseSensitivityX * (InvertMouseX ? -mouseX : mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(CharacterRotation);
        cameraHolder.localRotation = Quaternion.Euler(CameraRotation);
    }
}
