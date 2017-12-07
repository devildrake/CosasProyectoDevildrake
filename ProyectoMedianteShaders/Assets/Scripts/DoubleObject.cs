using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de la que heredan todos los GameObjects que tengan modo Dawn y modo Dusk, el método InitTransformable() debe ser llamado en el Start, 
//y este tiene un procedimiento estándar, no debería implementarse en los herederos, inicializa los boolenaos y llama a los métodos 
//Change y LoadResources deben ser implementados los herederos de forma obligatoria

//Y

//Clase de la que heredaran aquellos objetos transformables que tengan que modifiar su 
//comportamiento pero que solo pertenezcan a un mundo al mismo tiempo, no como el personaje
//la clase DoubleObject hereda de Transformable

public class DoubleObject : MonoBehaviour {

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

    //booleano para gestionar si el objeto ha sido añadido a la lista de transformables
    protected bool added;

    //Booleano para gestionar si se encuentra en dusk o en dawn
    public bool dawn;

    //Sprites distintos para cada mundo
    public Sprite imagenDusk;
    public Sprite imagenDawn;

    protected Vector2 originalPos;

    public virtual void GetBroken() {
        Debug.Log("Calling Get Broken");
    }

    //Método de comportamiento entre hermanos
    protected virtual void BrotherBehavior() {
        Debug.Log("Falta implementar BrotherBehavior");
    }

    //Método pseudo start que debe llamarse en el Start de cada heredero
    protected virtual void InitTransformable()
    {
        //isPunchable = false;
        added = false;
        dawn = true;
        LoadResources();
        Change();
        originalPos = transform.position;
    }

    protected virtual void OnlyFreezeRotation()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //Método que debe estar en el update de los herederos que comprueba si este objeto ha sido añadido a la lista de objetos transformables
    protected virtual void AddToGameLogicList()
    {
        if (!added)
        {
            if (GameLogic.instance != null)
            {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
            }
        }
    }

    //Método que carga las imagenes de los resources
    //Si no se implementa en alguna clase heredera se ejecuta el debug
    protected virtual void LoadResources()
    {
        Debug.Log("LoadResources esta vacío");
    }

    public virtual void Kill()
    {
        transform.position = originalPos;
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    //Método que se llama desde GameLogic con el listado de objetos transformables, cambia el booleano dawn y a su vez en función de si se pasa
    //a dawn = true o dawn = false se cambia el sprite a imagenDusk/imagenDawn
    //Si no se implementa en alguna clase heredera se ejecuta el debug
    public virtual void Change()
    {
        Debug.Log("Change esta vacío");
        dawn = !dawn;
    }

    //Al destruir este objeto se elimina de la lista de transformables
    private void OnDestroy()
    {
        if (GameLogic.instance != null) GameLogic.instance.transformableObjects.Remove(gameObject);
    }
}



