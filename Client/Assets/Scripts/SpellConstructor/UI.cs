using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject node;

    public void CreateNode () {
        Controller.Instance.carryNode(node);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
