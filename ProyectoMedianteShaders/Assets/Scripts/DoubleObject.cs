using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de la que heredaran aquellos objetos transformables que tengan que modifiar su 
//comportamiento pero que solo pertenezcan a un mundo al mismo tiempo, no como el personaje
//la clase DoubleObject hereda de Transformable

public class DoubleObject : Transformable {

    //Enum que diferenciará los dos mundos de una forma visual
    public enum world {DUSK,DAWN};

    //Asignación del enum para diferenciar los dos mundos de una forma visual
    public world worldAssignation;

    //Objeto hermano
    public GameObject brotherObject;

    //Offset, debe ser asignado a GameLogic.instance.worldOffset
    protected float offset;

    //Velocidad dominante, solo la utilizan los objetos que deben enviarse de forma amorosa la velocidad
    public Vector2 dominantVelocity;

    public bool isBreakable;
    public bool interactuableBySmash;
    public bool isPunchable;
    public bool isMovable;

    public virtual void GetBroken() {
        Debug.Log("Calling Get Broken");
    }

    //Método de comportamiento entre hermanos
    protected virtual void BrotherBehavior() {
        Debug.Log("Falta implementar BrotherBehavior");
    }

}
