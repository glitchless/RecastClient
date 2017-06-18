using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : Singleton<Controller> {
    GameObject carriedNode;
    bool carryingNode;
    int UILayer;
    int NodeLayer;
    int DefaultLayer = 0;
    bool showError = false;
    string errorText;
    float errorTiming = 0;

    void Start () {
        UILayer = LayerMask.NameToLayer("UI");
        NodeLayer = LayerMask.NameToLayer("Nodes"); 
        Debug.Log("UILayer =" + UILayer);
    }

    void OnGUI()
    {
        if (showError) {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 6, Screen.height / 6), errorText);
        }
    }
    // Update is called once per frame
    void Update () {
        if (carryingNode) {
            nodeDragUpdate();
        }
        if (showError) {
            if (errorTiming < 5f)
                errorTiming += Time.deltaTime;
            else {
                errorTiming = 0;
                showError = false;
            }
        }
    }

    void nodeDragUpdate() {
        var canvas = GetComponent<Canvas>();
        carriedNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane * 5));

        if (Input.GetMouseButtonDown(0)) {
            placeNode();
        }

        if (Input.GetMouseButtonDown(2)) {
            carryingNode = false;
            Destroy(carriedNode);
        }
    }

    void placeNode () {
        //layerMask = ~layerMask; //invert the mask so it targets all layers EXCEPT for this one
        PointerEventData cursor = new PointerEventData(EventSystem.current);                            // This section prepares a list for all objects hit with the raycast
        cursor.position = Input.mousePosition;
        List<RaycastResult> objectsHit = new List<RaycastResult>();
        EventSystem.current.RaycastAll(cursor, objectsHit);
        int hitsToUi = objectsHit.Count;
        if (hitsToUi == 0) {
            RaycastHit hit;
            float distance = 3f; //however far your ray shoots
            int layerMask = 1 << NodeLayer;
            //layerMask = ~layerMask; //invert layerMask
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, distance, layerMask)) {
                carryingNode = false;
                carriedNode.layer = NodeLayer;
            }
            else {
                showError = true;
                errorText = "You are placing this node too close to an another node"
            }//GUI.Label(new Rect(Screen.width/2, Screen.height / 2, Screen.width / 6, Screen.height / 6), "You are placing this node too close to an another node");//Debug.Log("You are placing this node too close to an another node");

        }
        else {
            Debug.Log("You are hitting a UI element");
        }
    }

    public bool carryNode(GameObject node) {
        if (node.GetComponentInChildren<NodeBehaviour>()) {
            if (!carryingNode) {
                carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
                carriedNode.layer = DefaultLayer;
                carryingNode = true;
                return true;
            }
            else {
                Destroy(carriedNode);
                carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
                carriedNode.layer = DefaultLayer;
                carryingNode = true;
                return true;
            }
        }
        return false;
    }
}
