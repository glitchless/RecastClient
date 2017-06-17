using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject node;
    GameObject carriedNode;
    bool carryingNode;
    // Use this for initialization
    public void CreateNode ()
    {
        carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);//new Vector3(0, 12, 0), Quaternion.identity);
        carryingNode = true;
        Debug.Log("WTF");
    }

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
}
