using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dash  {
    /*
     * Parametros que recibe
     *      PJ: personaje al cual se le aplicara la fuerza
     *      direction: vector normalizado de la dirección donde se aplicará la fuerza.
     *      MAX_FORCE: escalar de la fuerza que queremos que se aplique al dash
     *      
     * Este metodo aplica un impulso al personaje y provoca que haga un dash mediante el 
     * uso de fisicas.
     */
    public static void DoDash(GameObject PJ, Vector2 direction, float MAX_FORCE) {
        Debug.Log("DoDash" + direction);
        Rigidbody2D rigidbody = PJ.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.AddForce(rigidbody.velocity, ForceMode2D.Impulse);
        rigidbody.AddForce(direction*MAX_FORCE,ForceMode2D.Impulse);
        

    }
}
