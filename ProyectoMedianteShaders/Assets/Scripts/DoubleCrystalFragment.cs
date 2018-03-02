using UnityEngine;
using FragmentDataNamespace;
//using System.Collections;

public class DoubleCrystalFragment : DoubleObject {
    // Use this for initialization
    FragmentData data;
    //Rigidbody2D rb;
    public float angularSpeed;
    public Mesh mesh;
    void Start() {
        angularSpeed = 20;
        InitTransformable();
        isPunchable = false;
        isMovable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
       // if (worldAssignation == world.DAWN) {
           // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
       // } 
        //rb = GetComponent<Rigidbody2D>();

        //rb.mass = 5000;
    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
        //if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic) {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        //}

    }

    protected override void LoadResources() {
        //if (worldAssignation == world.DAWN) {
        //    imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnBox");
        //} else {
        //    imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBox");

        //}
        if (worldAssignation == world.DAWN) {
            int randomVal = Random.Range(1, 25);
            mesh = Resources.Load<Mesh>("Models/MirrorFrags/frag" + (randomVal.ToString()));
            GetComponent<MeshFilter>().mesh = mesh;
            brotherObject.GetComponent<MeshFilter>().mesh = mesh;
        }
        transform.Rotate(new Vector3(1, 0, 0), 90);
        transform.localScale = new Vector3(2,2,2);
    }

    public override void Change() {
        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn


            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
        }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            //GameLogic.instance.GrabFragment(data);
            collision.GetComponent<PlayerController>().crystalFragment = data;
            GameLogic.instance.SafelyDestroy(this);
        }
    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                offset = GameLogic.instance.worldOffset;
                    data = new FragmentData(false,GameLogic.instance.GetCurrentLevel());
                //Debug.Log(GameLogic.instance.GetCurrentLevel());
                    GameLogic.instance.AddFragmentData(data);

            }                Debug.Log("adding " + data.levelName + " " + data.picked);
        }
    }

    //GUARDAR FRAGMENT

    void CheckPick() {
        if (data.picked) {
            //Debug.Log(this);
            //AQUI FALTA HACER QUE SE GUARDE 
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

        transform.Rotate(new Vector3(0, 0, 1), angularSpeed * Time.deltaTime);

    }
}
