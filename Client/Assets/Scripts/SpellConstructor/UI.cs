using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject node;
    public GameObject carriedNode;
    bool carryingNode;
    // Use this for initialization
    public void CreateNode ()
    {
        carryingNode = false;
        var newNode = Instantiate(node, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);//new Vector3(0, 12, 0), Quaternion.identity);
        Debug.Log("WTF");
    }

    // Update is called once per frame
    void Update () {
		while
	}
}
