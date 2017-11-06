using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformableBox : Transformable {

	// Use this for initialization
	void Start () {
        AddToGameLogicList();
        InitTransformable();
        isPunchable = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
