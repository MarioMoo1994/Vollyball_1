using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigator : MonoBehaviour
{
    public List<Button> BtnList = new List<Button>();
    public Button ReturnBtn;
    public Button PanelBtn;
    public bool Panel = false;
    public Button CurrentBtn;
    [SerializeField] EventSystem eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ResetBtn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToPanel(GameObject btn) {
        eventSystem.SetSelectedGameObject(btn);
    }

    public void Return(GameObject btn) {
        eventSystem.SetSelectedGameObject(btn);
    }

    //public void MoveLeft() {

    //    if (CurrentBtn != BtnList[0] && !Panel) {
    //        int index = BtnList.IndexOf(CurrentBtn);
    //        CurrentBtn = BtnList[index - 1];
    //        eventSystem.SetSelectedGameObject(CurrentBtn.gameObject);
    //    }
    //}

    //public void MoveRight()
    //{
    //    if (CurrentBtn != BtnList[^1] && !Panel)
    //    {
    //        int index = BtnList.IndexOf(CurrentBtn);
    //        CurrentBtn = BtnList[index + 1];
    //        eventSystem.SetSelectedGameObject(CurrentBtn.gameObject);
    //    }
    //}

    //public void Press()
    //{
    //    if (CurrentBtn != null) { 
    //        CurrentBtn.onClick.Invoke();
    //    }

    //    if (Panel && CurrentBtn == ReturnBtn)
    //    {
    //        ResetBtn();
    //        Panel = false;
    //    }

    //    if (!Panel && CurrentBtn == PanelBtn) {
    //        PanelChange();
    //    }
    //}

    //public void PanelChange() {
    //    if (!Panel)
    //    {
    //        Panel = true;
    //        CurrentBtn = ReturnBtn;
    //        eventSystem.SetSelectedGameObject(CurrentBtn.gameObject);
    //    }
    //}

    //public void ResetBtn() {
    //    CurrentBtn = BtnList[0];
    //    eventSystem.SetSelectedGameObject(CurrentBtn.gameObject);
    //}
}
