using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MainMenuInput : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions_player1;
    //[SerializeField] InputActionAsset inputActions_player2;

    [Header("Any Controller")]
    [SerializeField] UnityEvent<InputAction.CallbackContext> controller_Left;
    [SerializeField] UnityEvent<InputAction.CallbackContext> controller_Right;
    [SerializeField] UnityEvent<InputAction.CallbackContext> controller_Press;
    readonly List<int> gamepadIds = new();


    void OnEnable()
    {
        var inputActionMap_player1 = inputActions_player1.FindActionMap("Map", true);
        inputActionMap_player1.Enable();

        BindInput(inputActionMap_player1.FindAction("MoveLeft", true), Input_ButtonSelectLeft);
        BindInput(inputActionMap_player1.FindAction("MoveRight", true), Input_ButtonSelectRight);
        BindInput(inputActionMap_player1.FindAction("Press", true), Input_ButtonSelectPress);

        //var inputActionMap_player2 = inputActions_player2.FindActionMap("Map", true);
        //inputActionMap_player2.Enable();

        //BindInput(inputActionMap_player2.FindAction("MoveLeft", true), Input_ButtonSelectLeft);
        //BindInput(inputActionMap_player2.FindAction("MoveRight", true), Input_ButtonSelectRight);
        //BindInput(inputActionMap_player2.FindAction("Press", true), Input_ButtonSelectPress);
    }

    void OnDisable()
    {
        var inputActionMap_player1 = inputActions_player1.FindActionMap("Map", true);
        inputActionMap_player1.Enable();

        UnbindInput(inputActionMap_player1.FindAction("MoveLeft", true), Input_ButtonSelectLeft);
        UnbindInput(inputActionMap_player1.FindAction("MoveRight", true), Input_ButtonSelectRight);
        UnbindInput(inputActionMap_player1.FindAction("Press", true), Input_ButtonSelectPress);

        //var inputActionMap_player2 = inputActions_player2.FindActionMap("Map", true);
        //inputActionMap_player2.Enable();

        //UnbindInput(inputActionMap_player2.FindAction("MoveLeft", true), Input_ButtonSelectLeft);
        //UnbindInput(inputActionMap_player2.FindAction("MoveRight", true), Input_ButtonSelectRight);
        //UnbindInput(inputActionMap_player2.FindAction("Press", true), Input_ButtonSelectPress);
    }

    static void BindInput(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    static void UnbindInput(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }

    void Input_ButtonSelectLeft(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed) return;
        controller_Left.Invoke(ctx);
    }

    void Input_ButtonSelectRight(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        controller_Right.Invoke(ctx);
    }

    void Input_ButtonSelectPress(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        controller_Press.Invoke(ctx);
    }
}
