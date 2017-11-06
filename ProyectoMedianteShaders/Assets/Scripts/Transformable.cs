using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase de la que heredan todos los GameObjects que tengan modo Dawn y modo Dusk, el método InitTransformable() debe ser llamado en el Start, 
//y este tiene un procedimiento estándar, no debería implementarse en los herederos, inicializa los boolenaos y llama a los métodos 
//Change y LoadResources deben ser implementados los herederos de forma obligatoria

public class Transformable : MonoBehaviour{


    //booleano para gestionar si el objeto ha sido añadido a la lista de transformables
    protected bool added;

    //Booleano para gestionar si se encuentra en dusk o en dawn
    public bool dawn;

    //Sprites distintos para cada mundo
    public Sprite imagenDusk;
    public Sprite imagenDawn;

    //Método pseudo start que debe llamarse en el Start de cada heredero
    protected virtual void InitTransformable() {
        //isPunchable = false;
        added = false;
        dawn = true;
        LoadResources();
        Change();
    }

    protected virtual void OnlyFreezeRotation() {
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //Método que debe estar en el update de los herederos que comprueba si este objeto ha sido añadido a la lista de objetos transformables
    protected virtual void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
            }
        }
    }

    //Método que carga las imagenes de los resources
    //Si no se implementa en alguna clase heredera se ejecuta el debug
    protected virtual void LoadResources() {
        Debug.Log("LoadResources esta vacío");
    }


    //Método que se llama desde GameLogic con el listado de objetos transformables, cambia el booleano dawn y a su vez en función de si se pasa
    //a dawn = true o dawn = false se cambia el sprite a imagenDusk/imagenDawn
    //Si no se implementa en alguna clase heredera se ejecuta el debug
    public virtual void Change() {
        Debug.Log("Change esta vacío");
    }

    //Al destruir este objeto se elimina de la lista de transformables
    private void OnDestroy() {
        if(GameLogic.instance!=null)GameLogic.instance.transformableObjects.Remove(gameObject);
    }
}
