using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject commonNode;
    public GameObject energyNode;
    public GameObject heaterNode;
    public GameObject aimNode;

    void Start () {
        Controller.Instance.initiate();
    }

    public void CreateCommonNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode<SpellNode>(commonNode));
        }
    }

    public void CreateEnergyNode () {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode<EnergyNode>(energyNode));
        }
    }

    public void CreateHeaterNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode<HeaterNode>(heaterNode));
        }
    }

    public void CreateAimNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Debug.Assert(Controller.Instance.carryNode<AimNode>(aimNode));
        }
    }

    public void SubmitSpell () {
        foreach (SpellNode node in Controller.Instance.getSpellNodes()) {
            var nodeType = node.GetType().ToString();

            uint nodeTypeID;
            if (SpellNode.typeToId.TryGetValue(nodeType, out nodeTypeID)) {
                Debug.Log(nodeTypeID);
            }
            else
                throw new System.Exception("This node's type does not correlate to ID");
            byte [] typeID_byte = BitConverter.GetBytes(nodeTypeID);
            byte[] linkID_byte;
            foreach (Link link in node.links) {
                Debug.Log(link.endId);
                linkID_byte = BitConverter.GetBytes(link.endId);
                //spell_byte = spell_byte.
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
