using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script pertenece a 
public class ImpulsingAir : MonoBehaviour{
    //Lista de objetos que estan dentro de la zona de acción del viento
    public List<GameObject> inTriggerZoneObjects;

    //Referencia al sistema de particulas
    public ParticleSystem windParticles;

    //Booleano que se guarda el hecho de que ha habido un cambio de mundo
    public bool changed;

    //Velocidad inicial del viento
    public float windSpeed=-0.4f;

    //Booleano que tweekea la subida/bajada del viento
    bool rising;

    //Booleano que indica si el objeto debe estar activo (en el sentido de que puede ser posible que lo esté por worldAssignation)
    bool active;

    // Use this for initialization
    void Start () {
        //Solo puede estar activo en dawn
        if (GetComponentInParent<DoubleObject>().worldAssignation == DoubleObject.world.DAWN)
        active = true;

        //El sistema de particulas inicia apagado
        windParticles.Stop();
    }

    public void RestartWind(){
        windSpeed = 0;
        rising = true;
    }
      

    void Update() {
        // Se comprueba si puede estar activo (active) y si ha sido activado (activated) mediante algun tipo de switch y en caso afirmativo pasa todo 
        if (active && gameObject.GetComponentInParent<DoubleObject>().activated) {
            
            //En función del booleano la velocidad del viento sube o bajas
            if (rising){
                if (windSpeed < -0.4f){
                    Debug.Log("Pasa a restar");
                    rising = false;
                }else{
                    windSpeed -= 0.5f * Time.deltaTime;
                }
            }
            else{
                if (windSpeed > 0.4f){
                    Debug.Log("Pasa a sumar");
                    rising = true;
                }
                else{
                    windSpeed += 0.7f * Time.deltaTime;
                }

            }

            //En caso de que haya habido un cambio de mundo
            if (changed){
                windParticles.Play();

                //Si hay alguno objeto y alguno es el jugador se cambia la variable onImpulsor y se limpia la lista
                if (inTriggerZoneObjects.Count > 0) {
                    foreach (GameObject g in inTriggerZoneObjects){
                        g.GetComponent<Rigidbody2D>().gravityScale = 1;
                        if (g.tag == "Player"){
                            g.GetComponent<PlayerController>().onImpulsor = false;
                        }
                    }
                    inTriggerZoneObjects.Clear();
                    changed = false;
                }
            }

            //En caso de que haya algún objeto dentro de la lista
            if (inTriggerZoneObjects.Count != 0){

                //Se comprueba de forma constante cual es el objeto más cercano y se modifica closestItem en consecuencia
                GameObject closestItem = inTriggerZoneObjects[0];

                foreach (GameObject g in inTriggerZoneObjects){
                    if (Vector3.Distance(transform.position, g.transform.position) < Vector3.Distance(transform.position, closestItem.transform.position)){
                        closestItem.GetComponent<Rigidbody2D>().gravityScale = 1;
                        closestItem = g;
                    }
                }

                //Se añade una fuerza al objeto más cercano siempre y cuando el viento sea positivo y el juego no este pausado
                if(windSpeed>0&&!GameLogic.instance.isPaused)
                closestItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * windSpeed * closestItem.GetComponent<Rigidbody2D>().mass, ForceMode2D.Impulse);

                windParticles.trigger.SetCollider(0,closestItem.GetComponent<Rigidbody2D>());
            }
            //windParticles.startSpeed = (windSpeed-(-0.4f)) * (1000-250) / (0.4f - -0.4f) + 250;
            //windParticles.maxParticles = (int)((windSpeed - (-0.4f)) * (1000 - 0) / (0.4f - -0.4f) + 0);
            //windParticles.startLifetime = (int)((windSpeed - (0.4f)) * (0.33f - 0.33f) / (-0.4f - 0.4f) + 0.33f);
        }
    }


//Cuando un objeto con rigidbody2D entra en el trigger se añade a la lista, si es el jugador se pone en true el bool onImpulsor
private void OnTriggerEnter2D(Collider2D collision)
{
    
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (!inTriggerZoneObjects.Contains(collision.gameObject))
            {
                inTriggerZoneObjects.Add(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = true;
                }
            
        }
    }
}

//Cuando un objeto con rigidbody2D dentro del trigger se mueve y no esta en la lista, se añade a la lista, si es el jugador se pone en true el bool onImpulsor
    private void OnTriggerStay2D(Collider2D collision)
{
    
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (!inTriggerZoneObjects.Contains(collision.gameObject))
            {
                inTriggerZoneObjects.Add(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = true;
                }
            }
        
    }
}

 //Cuando un objeto sale del trigger, se le borra de la lista y en caso de ser el jugador se pone en false el bool onImpulsor
private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (inTriggerZoneObjects.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                inTriggerZoneObjects.Remove(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = false;
                }
            }
        
    }
}
}
