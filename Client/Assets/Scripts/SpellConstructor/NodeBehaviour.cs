using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    SpellNode node;


	// Use this for initialization
	void Start () {
        node = new EnergyNode(EnergyNode.standartCapacity);
        this.transform.position = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
