using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavMenuItem : MonoBehaviour {
    public NavMenuItem upItem, downItem, rightItem, leftItem;
    public GameObject highlight;
    public Selectable selectableElement1, selectableElement2; //mis sliders tienen dos botones, para ir a izquierda o derecha.
    private enum MENU_ITEM_TYPE {BUTTON, SLIDER, MY_SLIDER, TOGGLE};
    public enum OPTION_TYPE {VIDEO, AUDIO};
    [SerializeField] private MENU_ITEM_TYPE myType;
    public OPTION_TYPE optionsPart;
    private Button button1, button2;
    private Slider slider;
    private Toggle toggle;

	void Start () {
        highlight.SetActive(false);
        //se coge la referencia del interactuable en qüestion
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
        }
	}

    public NavMenuItem DownElement() {
        if (downItem != null) {
            downItem.highlight.SetActive(true);
            highlight.SetActive(false);
            return downItem;
        }
        else {
            return this;
        }
    }

    public NavMenuItem UpElement() {
        if (upItem != null) {
            upItem.highlight.SetActive(true);
            highlight.SetActive(false);
            return upItem;
        }
        else {
            return this;
        }
    }

    public NavMenuItem RightElement() {
        return this;
    }

    public NavMenuItem LeftElement() {
        return this;
    }

    public void InteractButton() {
        switch (myType) {
            case MENU_ITEM_TYPE.BUTTON:
                button1.onClick.Invoke();
                break;
            case MENU_ITEM_TYPE.TOGGLE:
                toggle.isOn = !toggle.isOn;
                break;
        }
    }

    public void InteractRight(float offset = 0.05f) {
        switch (myType) {
            case MENU_ITEM_TYPE.SLIDER:
                slider.value += offset;
                break;
            case MENU_ITEM_TYPE.MY_SLIDER:
                button2.onClick.Invoke();
                break;
        }
    }

    public void InteractLeft(float offset = 0.05f) {
        switch (myType) {
            case MENU_ITEM_TYPE.SLIDER:
                slider.value -= offset;
                break;
            case MENU_ITEM_TYPE.MY_SLIDER:
                button1.onClick.Invoke();
                break;
        }
    }

    private void OnDisable() {
        highlight.SetActive(false);
    }
}
