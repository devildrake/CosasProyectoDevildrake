using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavItemBehavior : MonoBehaviour {

    public abstract void Try();
    public abstract NavItem UpElement(int i = 0);
    public abstract NavItem DownElement(int i = 0);
    public abstract NavItem RightElement();
    public abstract NavItem LeftElement();
}
