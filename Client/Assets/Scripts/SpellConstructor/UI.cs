using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject commonNode;
    public GameObject energyNode;
    public GameObject heaterNode;
    public GameObject aimNode;

    void Start () {
        Controller.Instance.initiate();
        commonNode = Instantiate(commonNode);
        commonNode.GetComponent<NodeBehaviour>().node.type = SpellNode.types.commonNode;
        energyNode.GetComponent<NodeBehaviour>().node.type = SpellNode.types.energyNode;
        heaterNode.GetComponent<NodeBehaviour>().node.type = SpellNode.types.heaterNode;
        aimNode.GetComponent<NodeBehaviour>().node.type = SpellNode.types.aimNode;
    }

    public void CreateCommonNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(commonNode));
        }
    }

    public void CreateEnergyNode () {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(energyNode));
        }
    }

    public void CreateHeaterNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(heaterNode));
        }
    }

    public void CreateAimNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode(aimNode));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
