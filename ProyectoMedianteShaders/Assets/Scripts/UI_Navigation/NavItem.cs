using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NavItem : MonoBehaviour {
    //downItem2 porque las setas pueden bajar a dos opciones dependiendo que está desplegado
    //upItem2 porque los botones de aceptar y cancelar tambien pueden ir hacia arriba a dos sitios.
    public NavItem upItem, upItem2, downItem, downItem2, rightItem, leftItem;
    public GameObject highlight;
    public Selectable selectableElement1, selectableElement2; //mis sliders tienen dos botones, para ir a izquierda o derecha.

    public enum NAV_TYPE { SIMPLE, MAIN_MENU, OPTIONS }
    public enum MENU_ITEM_TYPE { SHROOM_BUTTON, BUTTON, SLIDER, MY_SLIDER, TOGGLE };
    public enum OPTION_TYPE { SELECTOR, VIDEO, AUDIO };

    public MENU_ITEM_TYPE myType;
    public OPTION_TYPE optionsPart;
    public NAV_TYPE navType;
    protected Button button1, button2;
    protected Slider slider;
    protected Toggle toggle;

    //private GameObject eventSystem; //Referencia al event system para poder deseleccionar los botones de aceptar y cancelar.
    private EventSystem deselectButtons;
    private NavItemBehavior i;

    private void Awake() {
        if (highlight != null) {
            highlight.SetActive(false);
        }

        switch (navType){
            case NAV_TYPE.SIMPLE:
                i = new NavItem_Simple();
                break;
            case NAV_TYPE.OPTIONS:
                i = new NavItem_Options();
                break;
            case NAV_TYPE.MAIN_MENU:
                i = new NavItem_MainMenu();
                break;
        }
    }

    void Start () {
        deselectButtons = FindObjectOfType<EventSystem>();
        //deselectButtons.GetComponent<EventSystem>();
        if (navType == NAV_TYPE.SIMPLE) {
            myType = MENU_ITEM_TYPE.BUTTON;
            button1 = GetComponent<Button>();
        }
        else {
            //se coge la referencia del interactuable en cuestion
            switch (myType) {
                case MENU_ITEM_TYPE.BUTTON:
                    button1 = selectableElement1.GetComponent<Button>();
                    break;
                case MENU_ITEM_TYPE.TOGGLE:
                    toggle = selectableElement1.GetComponent<Toggle>();
                    break;
                case MENU_ITEM_TYPE.SLIDER:
                    slider = selectableElement1.GetComponent<Slider>();
                    break;
                case MENU_ITEM_TYPE.MY_SLIDER:
                    button1 = selectableElement1.GetComponent<Button>();
                    button2 = selectableElement2.GetComponent<Button>();
                    break;
                case MENU_ITEM_TYPE.SHROOM_BUTTON:
                    button1 = selectableElement1.GetComponent<Button>();
                    break;
            }
        }
	}

    //to = 0 --> si tiene que bajar a la primera opcion de audio o a cualquier otro item
    //to = 1 --> si tiene que bajar a la primera opcion de video
    public NavItem DownElement(int to = 0) {
        switch (navType) {
            case NAV_TYPE.SIMPLE:
                return i.DownElement(0,this);
            case NAV_TYPE.OPTIONS:
                if (to == 0) {
                    if (downItem != null) {
                        if (myType == MENU_ITEM_TYPE.BUTTON) {
                            deselectButtons.SetSelectedGameObject(null);
                        }
                        else {
                            highlight.SetActive(false);
                        }
                        if (downItem.myType == MENU_ITEM_TYPE.BUTTON) {
                            downItem.button1.Select();
                        }
                        else {
                            downItem.highlight.SetActive(true);
                        }

                        return downItem;
                    }
                    else {
                        return this;
                    }
                }
                else if (to == 1) {
                    if (downItem2 != null) {
                        if (myType == MENU_ITEM_TYPE.BUTTON) {
                            deselectButtons.SetSelectedGameObject(null);
                        }
                        else {
                            highlight.SetActive(false);
                        }
                        if (downItem2.myType == MENU_ITEM_TYPE.BUTTON) {
                            downItem2.button1.Select();
                        }
                        else {
                            downItem2.highlight.SetActive(true);
                        }
                        return downItem2;
                    }
                    else {
                        return this;
                    }
                }
                return this;
            default:
                return null;
        }
    }

    //to = 0 --> funcionamiento normal
    //to = 1 --> Tiene que subir a la ultima opcion de video.
    public NavItem UpElement(int to = 0) {
        switch (navType) {
            case NAV_TYPE.SIMPLE:
                return i.UpElement(0, this);

            case NAV_TYPE.OPTIONS:
                if (to == 0) {
                    if (upItem != null) {
                        if (myType == MENU_ITEM_TYPE.BUTTON) {
                            deselectButtons.SetSelectedGameObject(null);
                        }
                        else {
                            highlight.SetActive(false);
                        }
                        upItem.highlight.SetActive(true);
                        return upItem;
                    }
                    else {
                        return this;
                    }
                }
                else {
                    if (upItem2 != null) {
                        //solo necesito comprobar el mio porque si voy hacia arriba no puede haber otro botón al que le pueda hacer Select()
                        if (myType == MENU_ITEM_TYPE.BUTTON) {
                            deselectButtons.SetSelectedGameObject(null);
                        }
                        else {
                            highlight.SetActive(false);
                        }
                        upItem2.highlight.SetActive(true);
                        return upItem2;
                    }

                }
                return this;

            default:
                return null;
        }
        
    }

    public NavItem RightElement() {
        switch (navType) {
            case NAV_TYPE.SIMPLE:
                return i.RightElement(this);

            case NAV_TYPE.OPTIONS:
                if (rightItem != null) {
                    if (myType == MENU_ITEM_TYPE.BUTTON) {
                        deselectButtons.SetSelectedGameObject(null);
                    }
                    else {
                        highlight.SetActive(false);
                    }
                    if (rightItem.myType == MENU_ITEM_TYPE.BUTTON) {
                        rightItem.button1.Select();
                    }
                    else {
                        rightItem.highlight.SetActive(true);
                    }
                    return rightItem;
                }
                return this;

            default:
                return null;
        }
    }

    public NavItem LeftElement() {
        switch (navType) {
            case NAV_TYPE.SIMPLE:
                return i.LeftElement(this);
            case NAV_TYPE.OPTIONS:
                if (leftItem != null) {
                    if (myType == MENU_ITEM_TYPE.BUTTON) {
                        deselectButtons.SetSelectedGameObject(null);
                    }
                    else {
                        highlight.SetActive(false);
                    }
                    if (leftItem.myType == MENU_ITEM_TYPE.BUTTON) {
                        leftItem.button1.Select();
                    }
                    else {
                        leftItem.highlight.SetActive(true);
                    }
                    return leftItem;
                }
                return this;

            default:
                return null;
        }
        
    }

    public void InteractClick() {
        switch (myType) {
            case MENU_ITEM_TYPE.BUTTON:
                deselectButtons.SetSelectedGameObject(null);
                button1.onClick.Invoke();
                break;
            case MENU_ITEM_TYPE.TOGGLE:
                toggle.isOn = !toggle.isOn;
                break;
            case MENU_ITEM_TYPE.SHROOM_BUTTON:
                button1.onClick.Invoke();
                break;
        }
    }

    public void InteractRight(float offset = 0.5f) {
        switch (myType) {
            case MENU_ITEM_TYPE.SLIDER:
                slider.value += offset*0.02f;
                break;
            case MENU_ITEM_TYPE.MY_SLIDER:
                button2.onClick.Invoke();
                break;
        }
    }

    public void InteractLeft(float offset = 0.5f) {
        switch (myType) {
            case MENU_ITEM_TYPE.SLIDER:
                slider.value -= offset* 0.02f;
                break;
            case MENU_ITEM_TYPE.MY_SLIDER:
                button1.onClick.Invoke();
                break;
        }
    }

    private void OnDisable() {
        if (highlight != null) {
            highlight.SetActive(false);
        }
    }
}
