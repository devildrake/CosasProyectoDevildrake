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

    public NavItem initialItem;
    private NavItem currentItem;
    private EventSystem eventSystem;

    private Vector3 pMousePos;
    RaycastResult pRaycastTarget; //Item seleccionado en el frame anterior, se utiliza para comprovar el mismo elemento muchas veces.

    private GraphicRaycaster ray;

	void Start () {
        im = InputManager.instance;
        currentItem = initialItem;
        MouseOff();
        ray = GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        InputManager.UnBlockInput();
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
            MouseOff();
            kbUse = true;
        }

        //IZQUIERDA
        if ((im.horizontalAxis < 0 && im.prevHorizontalAxis == 0) ||
            (im.horizontalAxis2 < 0 && im.prevHorizontalAxis2 == 0) ||
            (im.leftKey && !im.prevLeftKey)) {
            currentItem = currentItem.LeftElement();
            MouseOff();
            kbUse = true;
        }
        //ARRIBA
        if ((im.verticalAxis < 0 && im.prevVerticalAxis == 0) ||
            (im.verticalAxis2 < 0 && im.prevVerticalAxis2 == 0) ||
            (im.upKey && !im.prevUpKey)) {
            currentItem = currentItem.UpElement();
            MouseOff();
            kbUse = true;
        }
        //ABAJO
        if ((im.verticalAxis > 0 && im.prevVerticalAxis == 0) ||
            (im.verticalAxis2 > 0 && im.prevVerticalAxis2 == 0) ||
            (im.downKey && !im.prevDownKey)) {
            currentItem = currentItem.DownElement();
            MouseOff();
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

            if (results.Count != 0) {
                if (!results[0].Equals(pRaycastTarget)) {
                    NavItem item = results[0].gameObject.GetComponent<NavItem>();
                    if (item != null) {
                        currentItem = item;
                    }
                }
            }

            eventSystem.SetSelectedGameObject(currentItem.gameObject);
            if(results.Count != 0 && !kbUse) pRaycastTarget = results[0];
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
        StartCoroutine(DisableMouse());
        kbUse = true;
    }

    private void MouseOn() {
        Cursor.visible = true;
        kbUse = false;
    }

    IEnumerator DisableMouse() {
        Cursor.visible = false;
        yield return null;
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }
}
