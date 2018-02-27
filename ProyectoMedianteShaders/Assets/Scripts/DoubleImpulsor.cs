using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleImpulsor : DoubleObject
{
    void Start()
    {
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN)
        {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }

    }

    protected override void BrotherBehavior()
    {
        
        if (worldAssignation == world.DAWN) {
            Vector3 positionWithOffset = brotherObject.transform.position;
            positionWithOffset.y += offset;
            transform.position = positionWithOffset;

        }
    }

    void BecomePunchable()
    {
        isPunchable = true;
    }

    protected override void LoadResources()
    {
        if (worldAssignation == world.DAWN)
        {
            imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnBox");
        }
        else
        {
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBox");
        }
    }

    public override void Change()
    {
        GetComponentInChildren<ImpulsingAir>().RestartWind();

        GetComponentInChildren<ImpulsingAir>().changed = true;
        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        
            dawn = !dawn;
        

    }

    public override void Activate()
    {
        base.Activate();
        //Change();
    }
    // Update is called once per frame
    void Update()
    {
        AddToGameLogicList();
        BrotherBehavior();
    }
}
