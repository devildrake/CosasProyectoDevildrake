using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private void Start() {
    }

    //Método que comprueba si la velocidad y del personaje es 0 o aprox. y actualiza el booleano grounded en consecuencia
    public void CheckGrounded(PlayerController player) {

        LayerMask[] mascaras = new LayerMask[1];
        mascaras[0] = LayerMask.GetMask("Walkable");
        //mascaras[1] = LayerMask.GetMask("Ground");
        //mascaras[2] = LayerMask.GetMask("Enemy");


        RaycastHit2D hit2D = PlayerUtilsStatic.RayCastArrayMask(transform.position, Vector3.down, 0.05f, mascaras);
        RaycastHit2D hit2DLeft = PlayerUtilsStatic.RayCastArrayMask(transform.position + new Vector3(-player.distanciaBordeSprite, 0, 0), Vector3.down, 0.05f, mascaras);
        RaycastHit2D hit2DRight = PlayerUtilsStatic.RayCastArrayMask(transform.position + new Vector3(player.distanciaBordeSprite, 0, 0), Vector3.down, 0.05f, mascaras);

        Debug.DrawLine(transform.position, Vector3.down * 0.05f);
        Debug.DrawLine(transform.position + new Vector3(-player.distanciaBordeSprite, 0, 0), Vector3.down * 0.05f);
        Debug.DrawLine(transform.position + new Vector3(player.distanciaBordeSprite, 0, 0), Vector3.down * 0.05f);


        //if (hit2D.collider != null) 
        //Debug.Log(hit2D.collider);

        //RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.1f, groundMask);
        //RaycastHit2D hit2DLeft = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
        //RaycastHit2D hit2DRight = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
        // If the raycast hit something
        if (hit2D || hit2DLeft || hit2DRight) {
            //if (player.GetComponent<Rigidbody2D>().velocity.y == 0) {

            if (!player.grounded) {
                player.grounded = true;
                //player.dashing = false;
                player.SetCanDash(true);
            }
            //}
        } else {
            player.grounded = false;
        }
        RaycastHit2D hit2DLeftO = Physics2D.Raycast(player.rb.position + new Vector2(-player.distanciaBordeSprite, 0), Vector2.down, 0.1f, player.groundMask);
        RaycastHit2D hit2DRightO = Physics2D.Raycast(player.rb.position  + new Vector2(player.distanciaBordeSprite, 0), Vector2.down, 0.1f, player.groundMask);

        if (!hit2DLeftO && !hit2DRightO) {
            bool right = true;

            //SLIDE PA LA DERECHA
            hit2DRightO = Physics2D.Raycast(player.rb.position  + new Vector2(player.distanciaBordeSprite, 0), Vector2.down, 3.0f, player.slideMask);
            hit2DLeftO = Physics2D.Raycast(player.rb.position + new Vector2(-player.distanciaBordeSprite, 0), Vector2.down, 0.2f, player.slideMask);


            if (!(hit2DLeftO && hit2DRightO)) {
                right = false;
                //SLIDE PA LA IZQUIERDA
                hit2DRightO = Physics2D.Raycast(player.rb.position + new Vector2(player.distanciaBordeSprite, 0), Vector2.down, 0.2f, player.slideMask);
                hit2DLeftO = Physics2D.Raycast(player.rb.position + new Vector2(-player.distanciaBordeSprite, 0), Vector2.down, 3.0f, player.slideMask);
            }
            if (hit2DLeftO && hit2DRightO) {
                if (!player.facingRight && right) {
                    player.Flip();
                    player.prevHorizontalMov = 1.0f;

                } else if (player.facingRight && !right) {
                    player.Flip();
                    player.prevHorizontalMov = -1.0f;

                }
                //Debug.Log(hit2DLeftO.collider.gameObject.transform.rotation.z);
                if (player.timeNotSliding > 0.3f) {
                    player.sliding = true;
                    player.rb.velocity = new Vector2(player.rb.velocity.x, -10);
                    player.timeNotSliding = 0;
                }
                //Debug.Log("BothHit");
            } else {
                player.sliding = false;
            }
        } else {
            player.sliding = false;
        }
    }
}
