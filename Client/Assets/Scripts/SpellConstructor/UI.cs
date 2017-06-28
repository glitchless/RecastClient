using System;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject commonNode;
    public GameObject energyNode;
    public GameObject heaterNode;
    public GameObject aimNode;
    public GameObject generatorNode;

    void Start () {
        Controller.Instance.initiate();
    }

    public void CreateCommonNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Controller.Instance.carryNode<SpellNode>(commonNode);
        }
    }

    public void CreateEnergyNode () {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Controller.Instance.carryNode<EnergyNode>(energyNode);
        }
    }

    public void CreateHeaterNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Controller.Instance.carryNode<HeaterNode>(heaterNode);
        }
    }

    public void CreateAimNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Controller.Instance.carryNode<AimNode>(aimNode);
        }
    }

    public void CreateGeneratorNode() {
        if (Controller.Instance.state != Controller.status.linkingNodes) {
            Controller.Instance.carryNode<GeneratorNode>(generatorNode);
        }
    }

    public void SubmitSpell () {
        const int UINT_SIZE = 4;
        const int FLOAT_SIZE = 4;
        uint counter = 0;
        foreach (SpellNode node in Controller.Instance.getSpellNodes()) {

            byte[] node_serialized = new byte[UINT_SIZE * 2 + FLOAT_SIZE * 3 + UINT_SIZE + UINT_SIZE * node.links.Count];

            byte[] counter_byte = BitConverter.GetBytes(counter);
            Buffer.BlockCopy(counter_byte, 0, node_serialized, 0, UINT_SIZE);

            var nodeType = node.GetType().ToString();
            uint nodeTypeID;

            if (SpellNode.typeToId.TryGetValue(nodeType, out nodeTypeID)) {
                Debug.Log(nodeTypeID);
            }
            else
                throw new Exception("This node's type does not correlate to ID");

            byte [] typeID_byte = BitConverter.GetBytes(nodeTypeID);
            Buffer.BlockCopy(typeID_byte, 0, node_serialized, UINT_SIZE, UINT_SIZE);

            byte[] coordX_byte = BitConverter.GetBytes(node.position.x);
            byte[] coordY_byte = BitConverter.GetBytes(node.position.y);
            byte[] coordZ_byte = BitConverter.GetBytes(node.position.z);

            Buffer.BlockCopy(coordX_byte, 0, node_serialized, UINT_SIZE * 2, FLOAT_SIZE);
            Buffer.BlockCopy(coordY_byte, 0, node_serialized, UINT_SIZE * 2 + FLOAT_SIZE, FLOAT_SIZE);
            Buffer.BlockCopy(coordZ_byte, 0, node_serialized, UINT_SIZE * 2 + FLOAT_SIZE * 2, FLOAT_SIZE);

            byte[] linkCount_byte = BitConverter.GetBytes(node.links.Count);
            Buffer.BlockCopy(typeID_byte, 0, node_serialized, UINT_SIZE * 2 + FLOAT_SIZE * 3, UINT_SIZE);

            byte[] linkID_byte;
            
            for (int i = 0; i < node.links.Count; i++) {
                Debug.Log(node.links[i].endId);
                linkID_byte = BitConverter.GetBytes(node.links[i].endId);
                Buffer.BlockCopy(typeID_byte, 0, node_serialized, UINT_SIZE * 2 + FLOAT_SIZE * 3 + i * UINT_SIZE, UINT_SIZE);
                //spell_byte = spell_byte.
            }
            counter++;
            Networking.send_tcp(Networking.DEFAULT_SERVER_HOSTNAME, Networking.DEFAULT_PORT_TCP_LISTEN, node_serialized, Networking.DEFAULT_NODE_LISTENER);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
