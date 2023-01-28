using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    private PlayerInput playerInput;

    private void Awake() {
        Instance = this;
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }

    public Vector2 GetMousePosition() => Mouse.current.position.ReadValue();


    //public bool GetMouseClick() => Mouse.current.leftButton.IsPressed(); this also work
    public bool GetMouseClick() => playerInput.Player.MouseClick.WasPressedThisFrame();

     public bool GetMouseClickRight() => playerInput.Player.MouseClickRight.WasPressedThisFrame();
    public Vector2 GetCameraMove() => playerInput.Player.CameraMove.ReadValue<Vector2>();
    public float GetCameraRotate() => playerInput.Player.CameraRotate.ReadValue<float>();

    public float GetCameraZoom() => playerInput.Player.CameraZoom.ReadValue<float>();
}
