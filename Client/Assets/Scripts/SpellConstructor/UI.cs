using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject powerNode;
    public GameObject emitterNode;

    public void CreatePowerNode () {
        Debug.Assert(Controller.Instance.carryNode(powerNode));
    }

    public void CreateEmitterNode() {
        Debug.Assert(Controller.Instance.carryNode(emitterNode));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
