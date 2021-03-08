using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavItem_Simple : NavItemBehavior {
    public override NavItem DownElement(int i = 0, NavItem item = null) {
        return item.downItem.Equals(null) ? item : item.downItem;
    }

    public override NavItem LeftElement(NavItem item) {
        return item.leftItem.Equals(null) ? item : item.leftItem;
    }

    public override NavItem RightElement(NavItem item) {
        return item.rightItem.Equals(null) ? item : item.rightItem;
    }

    public override NavItem UpElement(int i = 0, NavItem item = null) {
        return item.upItem.Equals(null) ? item : item.upItem;
    }
}
