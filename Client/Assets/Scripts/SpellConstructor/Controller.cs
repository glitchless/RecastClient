using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Controller : Singleton<Controller> {
    protected Controller() {}
    GameObject carriedNode;
    public enum status
    {
        none,
        carryingNode,
        linkingStart,
        linkingNodes,
        linkOnTarget,
        linked
    }
    public Link currentLink; 
    public status state = status.none;
    public GameObject linkBeginning;
    public GameObject linkTarget;
    int NodeLayer;
    status previousState;
    int DefaultLayer = 0;
    bool showError = false;
    string errorText;
    float errorTiming = 0;
    Vector3 mouseWorldPos;

    void Start () {
        //GameObject controller = this.gameObject;
        //Debug.Log(controller);
        //controller.AddComponent<EventCatcher>();
        state = status.none;
        previousState = status.none;
        NodeLayer = LayerMask.NameToLayer("Nodes"); 
    }

    void OnGUI()
    {
        if (showError) {
            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 6, Screen.height / 6), errorText);\
            GUILayout.Label("<size=30><color=red>" + errorText + "</color></size>");
        }
    }

    // Update is called once per frame
    void Update () {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane * 5));
        if (state != previousState) {
            Debug.Log(state);
            previousState = state;
        }
        switch (state) {
            case status.carryingNode:
                nodeDragUpdate();
                break;
            case status.linkingStart:
                linkingStart();
                break;
            case status.linkingNodes:
                linkingUpdate();
                break;
            case status.linkOnTarget:
                attachLink();
                break;
            case status.linked:
                finalizeLink();
                break;
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

    private void linkingStart()
    {
        LineRenderer link = linkBeginning.GetComponent<LineRenderer>();
        if (!link) {
            link = linkBeginning.AddComponent<LineRenderer>();
            link.useWorldSpace = true;
            link.positionCount = 0;
            link.widthMultiplier = 0.2f;
            link.material = new Material(Shader.Find("Particles/Additive"));
            //link.startColor = Color.gray;
            //link.endColor = Color.gray;
            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 1.0f;
            Color c1 = Color.yellow;
            Color c2 = Color.red;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            link.colorGradient = gradient;
        }
        link.positionCount += 2;
        currentLink.start = linkBeginning.transform.position;
        linkingUpdate();
        state = status.linkingNodes;
    }

    void linkingUpdate () {
        currentLink.end = mouseWorldPos;
        LineRenderer link = linkBeginning.GetComponent<LineRenderer>();

        //Debug.Log("StartWidth = " + link.startWidth);
        //Debug.Log("EndWidth = " + link.endWidth);
        //Debug.Log("StartColor= " + link.startColor);
        //Debug.Log("EndColor= " + link.endColor);
        link.SetPosition(link.positionCount - 2, currentLink.start);
        link.SetPosition(link.positionCount - 1, currentLink.end);
        if (Input.GetMouseButtonDown(2)) {
            state = status.none;
            link.positionCount -= 2;
        }
    }

    private void attachLink()
    {
        var link = linkBeginning.GetComponent<LineRenderer>();
        link.SetPosition(link.positionCount - 1, linkTarget.transform.position);
    }


    private void finalizeLink() {
        var currentLink = new Link(linkBeginning.transform.position, linkTarget.transform.position);
        var startNode = linkBeginning.GetComponent<NodeBehaviour>();
        var endNode = linkTarget.GetComponent<NodeBehaviour>();
        if (!startNode.node.links.Contains(currentLink) && !endNode.node.links.Contains(currentLink)) {
            Debug.Log("Link added");
            foreach (Link link in startNode.node.links) {
                Debug.Log("Link" +":" + link.start + " " + link.end);
                Debug.Log("Link is " + link.Equals(currentLink) + " to current");
            }
            startNode.node.links.Add(currentLink);
            endNode.node.links.Add(currentLink.reverse());
        }
        else {
            Debug.Log("Link terminated as obsolete");
            LineRenderer link = linkBeginning.GetComponent<LineRenderer>();
            link.positionCount -= 2;
        }
        state = status.none;
    }

    public bool carryNode(GameObject node) {
        if (node.GetComponentInChildren<NodeBehaviour>()) {
            if (state != status.carryingNode) {
                node.AddComponent<EventCatcher>();
                carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
                carriedNode.layer = DefaultLayer;
                state = status.carryingNode;
                return true;
            }
            else {
                Destroy(carriedNode);
                carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
                carriedNode.layer = DefaultLayer;
                state = status.carryingNode;
                return true;
            }
        }
        return false;
    }

    void nodeDragUpdate() {
        var canvas = GetComponent<Canvas>();
        carriedNode.transform.position = mouseWorldPos;

        if (Input.GetMouseButtonDown(0)) {
            placeNode();
        }

        if (Input.GetMouseButtonDown(2)) {
            state = status.none;
            Destroy(carriedNode);
        }
    }

    void placeNode () {
        PointerEventData cursor = new PointerEventData(EventSystem.current);
        cursor.position = Input.mousePosition;
        List<RaycastResult> objectsHit = new List<RaycastResult>(); // This section prepares a list for all objects hit with the raycast
        EventSystem.current.RaycastAll(cursor, objectsHit);
        int hitsToUi = objectsHit.Count;

        if (hitsToUi == 0) {
            RaycastHit hit;
            float distance = 3f; //however far your ray shoots
            int layerMask = 1 << NodeLayer;
            //layerMask = ~layerMask; //invert layerMask
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayRadius = NodeBehaviour.nodeRadius;
            if (!Physics.SphereCast(ray, rayRadius, out hit, distance, layerMask)) {
                state = status.none;
                carriedNode.layer = NodeLayer;
            }
            else {
                showError = true;
                errorText = "You are placing this node too close to an another node";
            }//GUI.Label(new Rect(Screen.width/2, Screen.height / 2, Screen.width / 6, Screen.height / 6), "You are placing this node too close to an another node");//Debug.Log("You are placing this node too close to an another node");

        }
        else {
            Debug.Log("You are hitting a UI element");
        }
    }
}
