using UnityEngine;

public class DoubleCrystalFragment : DoubleObject {
    //Velocidad angular para el giro bonico del fragmento
    public float angularSpeed;

    //Referencia al Mesh
    public Mesh mesh;

    void Start() {
        angularSpeed = 20;
        InitTransformable();
        isPunchable = false;
        isMovable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
    }

    //OFFSET
    protected override void BrotherBehavior() {
        if (worldAssignation == world.DAWN) {
            transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y + GameLogic.instance.worldOffset, brotherObject.transform.position.z);
        }
    }

    //Carga un modelo Random de todos los que hay 
    protected override void LoadResources() {
        if (worldAssignation == world.DAWN) {
            int randomVal = Random.Range(1, 25);
            mesh = Resources.Load<Mesh>("Models/MirrorFrags/frag" + (randomVal.ToString()));
            GetComponent<MeshFilter>().mesh = mesh;
            brotherObject.GetComponent<MeshFilter>().mesh = mesh;
        }
        transform.Rotate(new Vector3(1, 0, 0), 90);
        transform.localScale = new Vector3(2,2,2);
    }

    //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
    public override void Change() {
            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
        }

    //Colision de trigger para coger el Fragmento
    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player") {
            collision.GetComponent<PlayerController>().hasACrystal = true;
            GameLogic.instance.SafelyDestroy(this);
        }
    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                offset = GameLogic.instance.worldOffset;
            }
        }
    }

    //Comprovación de que este fragmento no haya sido cogido ya con anterioridad
    void CheckPick() {
        if (GameLogic.instance.levelsData[GameLogic.instance.GetCurrentLevelIndex()].fragment) {
            GameLogic.instance.SafelyDestroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        
        if (added) {
            CheckPick();
        }

        //Rotación over time para que quede bonito
        transform.Rotate(new Vector3(0, 0, 1), angularSpeed * Time.deltaTime);

    }
}
