using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject powerNode;
    public GameObject emitterNode;

    void Start () {
        Controller.Instance.initiate();
    }

    public void CreatePowerNode () {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(powerNode));
        }
    }

    public void CreateEmitterNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(emitterNode));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
