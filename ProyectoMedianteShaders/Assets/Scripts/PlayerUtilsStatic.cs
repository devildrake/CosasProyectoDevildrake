using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtilsStatic {
    public static void DoDash(GameObject PJ, Vector2 direction, float MAX_FORCE) {
        Rigidbody2D rigidbody = PJ.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.AddForce(rigidbody.velocity, ForceMode2D.Impulse);
        rigidbody.AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
    }
    public static void Punch(Vector2 direction, float MAX_FORCE, List<GameObject>NearbyObjects) {
        foreach (GameObject g in NearbyObjects) {
            if (g.GetComponent<DoubleObject>().isPunchable) {
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                g.GetComponent<Rigidbody2D>().AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
                g.GetComponent<DoubleObject>().isPunchable = false;
            }
        }

    }

    public static void Smash(GameObject g) {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        if (!g.GetComponent<PlayerController>().smashing) {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -700));
            g.GetComponent<PlayerController>().smashing = true;
        }
    }

    public static void DoSmash(GameObject g) {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f, g.GetComponent<PlayerController>().groundMask);
        if (hit2D) {
            Debug.Log("HittingStuff");
            if (hit2D.transform.gameObject.GetComponent<DoubleObject>().isBreakable) {
                hit2D.transform.gameObject.GetComponent<DoubleObject>().GetBroken();
            }
        }
    }
}
