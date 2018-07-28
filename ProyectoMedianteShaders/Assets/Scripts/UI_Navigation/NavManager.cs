using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class NavManager : MonoBehaviour {

    public bool block = false;
    public bool kbUse = true; //Cuando se mueve el raton se pone a false para no highlightear el currentItem, 
                               //cuando se vuelven a usar las flechas se vuelve a poner true
    private InputManager im;

    public NavMenuItem initialItem;
    private NavMenuItem currentItem;
    private EventSystem eventSystem;

    private Vector3 pMousePos;
    RaycastResult pRaycastTarget; //Item seleccionado en el frame anterior, se utiliza para comprovar el mismo elemento muchas veces.

    private GraphicRaycaster ray;

	void Start () {
        im = InputManager.instance;
        currentItem = initialItem;
        MouseOff();
        ray = GetComponent<GraphicRaycaster>();
    }
	
	// Update is called once per frame
	void Update (){
        if (block) return;

        #region MOVEMENT
        //DERECHA
        if ((im.horizontalAxis > 0 && im.prevHorizontalAxis == 0) ||
            (im.horizontalAxis2 > 0 && im.prevHorizontalAxis2 == 0) ||
            (im.rightKey && !im.prevRightKey)) {
            currentItem = currentItem.RightElement();
            kbUse = true;
        }

        //IZQUIERDA
        if ((im.horizontalAxis < 0 && im.prevHorizontalAxis == 0) ||
            (im.horizontalAxis2 < 0 && im.prevHorizontalAxis2 == 0) ||
            (im.leftKey && !im.prevLeftKey)) {
            currentItem = currentItem.LeftElement();
            kbUse = true;
        }
        //ARRIBA
        if ((im.verticalAxis < 0 && im.prevVerticalAxis == 0) ||
            (im.verticalAxis2 < 0 && im.prevVerticalAxis2 == 0) ||
            (im.upKey && !im.prevUpKey)) {
            currentItem = currentItem.UpElement();
            kbUse = true;
        }
        //ABAJO
        if ((im.verticalAxis > 0 && im.prevVerticalAxis == 0) ||
            (im.verticalAxis2 > 0 && im.prevVerticalAxis2 == 0) ||
            (im.downKey && !im.prevDownKey)) {
            currentItem = currentItem.DownElement();
            kbUse = true;
        }
        #endregion

        CheckMouse();

        if (kbUse) {
            pRaycastTarget.Clear();
        }
        else {
            PointerEventData ped = new PointerEventData(eventSystem);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            ray.Raycast(ped, results);

            if (!results[0].Equals(pRaycastTarget)) {
                NavMenuItem item = results[0].gameObject.GetComponent<NavMenuItem>();
                if (item != null) {
                    currentItem = item;
                }
            }

            eventSystem.SetSelectedGameObject(currentItem.gameObject);
            pRaycastTarget = results[0];
        }
    }

    private void CheckMouse() {
        if(pMousePos != Input.mousePosition) {
            MouseOn();
            kbUse = false;
        }
        pMousePos = Input.mousePosition;
    }

    private void MouseOff() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        kbUse = true;
    }

    private void MouseOn() {
        Cursor.visible = true;
        kbUse = false;
    }
}
