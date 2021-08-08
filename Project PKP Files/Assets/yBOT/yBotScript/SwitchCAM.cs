using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchCAM : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerinput;

    [SerializeField]
    private int priorityBoostAmount = 10;

    [SerializeField]
    private Canvas thirdpersoncanvas;

    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualcamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerinput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim(); 
    }

    private void StartAim()
    { 
        virtualcamera.Priority += priorityBoostAmount;
    }

    private void CancelAim()
    {
        virtualcamera.Priority -= priorityBoostAmount;
    }
}
