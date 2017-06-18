using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Context = ConstructorContext;

[System.Serializable]
public class Controller : Singleton<Controller> {
    GameObject carriedNode;
    bool carryingNode;
	
	// Update is called once per frame
	void Update () {
        if (carryingNode) {
            carriedNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane * 5));

            if (Input.GetMouseButtonDown(0))
                carryingNode = false;

            if (Input.GetMouseButtonDown(2)) {
                carryingNode = false;
                Destroy(carriedNode);
            }
        }
    }

    public bool carryNode(GameObject node) {
        if (node.GetComponentInChildren<NodeBehaviour>()) {
            carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
            carryingNode = true;
            return true;
        }
        else
            return false;
    }
}
