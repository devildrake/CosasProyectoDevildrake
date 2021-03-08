using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavItemBehavior : MonoBehaviour {

    public abstract NavItem UpElement(int i = 0, NavItem item = null);
    public abstract NavItem DownElement(int i = 0, NavItem item = null);
    public abstract NavItem RightElement(NavItem item);
    public abstract NavItem LeftElement(NavItem item);
}
