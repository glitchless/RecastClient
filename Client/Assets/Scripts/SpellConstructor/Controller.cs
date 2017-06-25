using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Controller : Singleton<Controller> {
    protected Controller() { }

    public status state = status.none;
    status previousState;
    public enum status {
        none,
        carryingNode,
        linkingStart,
        linkingNodes,
        linkOnTarget,
        linked
    }
    
    public Link currentLink;
    GameObject linkVisual;
    public GameObject linkBeginning;
    public GameObject linkTarget;
    GameObject carriedNode;
    List<SpellNode> nodesPlaced;
    int NodeLayer;
    int DefaultLayer = 0;
    bool showError = false;
    string errorText;
    float errorTiming = 0;
    Vector3 mouseWorldPos;

    void Start() {
        //GameObject controller = this.gameObject;
        //Debug.Log(controller);
        //controller.AddComponent<EventCatcher>();
        state = status.none;
        previousState = status.none;
        NodeLayer = LayerMask.NameToLayer("Nodes");
        nodesPlaced = new List<SpellNode>();
    }

    void OnGUI() {
        if (showError) {
            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 6, Screen.height / 6), errorText);\
            GUILayout.Label("<size=30><color=red>" + errorText + "</color></size>");
        }
    }

    // Update is called once per frame
    void LateUpdate() {
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

    private void linkingStart() {
        linkVisual = new GameObject("Link");
        linkVisual.transform.parent = linkBeginning.transform;
        //LineRenderer link = linkVisual.GetComponent<LineRenderer>();
        LineRenderer link;
        if (true) {
            link = linkVisual.AddComponent<LineRenderer>();
            link.useWorldSpace = true;
            link.positionCount = 2;
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
        currentLink.start = linkBeginning.transform.position;
        linkingUpdate();
        state = status.linkingNodes;
    }

    void linkingUpdate() {
        currentLink.end = mouseWorldPos;
        LineRenderer link = linkVisual.GetComponent<LineRenderer>();

        //Debug.Log("StartWidth = " + link.startWidth);
        //Debug.Log("EndWidth = " + link.endWidth);
        //Debug.Log("StartColor= " + link.startColor);
        //Debug.Log("EndColor= " + link.endColor);
        link.SetPosition(link.positionCount - 2, currentLink.start);
        link.SetPosition(link.positionCount - 1, currentLink.end);
        if (Input.GetMouseButtonDown(2)) {
            state = status.none;
            Destroy(linkVisual);
        }
    }

    private void attachLink() {
        var link = linkVisual.GetComponent<LineRenderer>();
        link.SetPosition(link.positionCount - 1, linkTarget.transform.position);
    }


    private void finalizeLink() {
        var currentLink = new Link(linkBeginning.transform.position, linkTarget.transform.position);
        var startNode = linkBeginning.GetComponent<NodeBehaviour>();
        var endNode = linkTarget.GetComponent<NodeBehaviour>();
        if (!startNode.node.links.Contains(currentLink) && !endNode.node.links.Contains(currentLink)) {
            Debug.Log("Link added");
            foreach (Link link in startNode.node.links) {
                Debug.Log("Link" + ":" + link.start + " " + link.end);
                Debug.Log("Link is " + link.Equals(currentLink) + " to current");
            }
            currentLink.endId = endNode.node.id;
            startNode.node.links.Add(currentLink);
            currentLink.endId = startNode.node.id;
            endNode.node.links.Add(currentLink.reverse());
        }
        else {
            Debug.Log("Link terminated as obsolete");
            //LineRenderer link = linkVisual.GetComponent<LineRenderer>();
            Destroy(linkVisual);
            //link.positionCount -= 2;
        }
        state = status.none;
    }

    public bool carryNode<T> (GameObject node) where T : SpellNode, new() {
        var nodeBehaviour = node.GetComponent<NodeBehaviour>();
        if (nodeBehaviour) {
            if (state == status.carryingNode) {
                Destroy(carriedNode);
            }
            carriedNode = Instantiate(node, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Quaternion.identity);
            nodeBehaviour = carriedNode.GetComponent<NodeBehaviour>();
            nodeBehaviour.node = new T();
            nodeBehaviour.node.id = (uint)nodesPlaced.Count;
            carriedNode.layer = DefaultLayer;
            state = status.carryingNode;
            return true;
        }
        else
            return false;
    }

    void nodeDragUpdate() {
        carriedNode.transform.position = mouseWorldPos;

        if (Input.GetMouseButtonDown(0)) {
            placeNode();
        }

        if (Input.GetMouseButtonDown(2)) {
            state = status.none;
            Destroy(carriedNode);
        }
    }

    void placeNode() {
        PointerEventData cursor = new PointerEventData(EventSystem.current);
        cursor.position = Input.mousePosition;
        List<RaycastResult> objectsHit = new List<RaycastResult>(); // This section prepares a list for all objects hit with the raycast
        EventSystem.current.RaycastAll(cursor, objectsHit);
        int hitsToUi = objectsHit.Count;

        if (hitsToUi == 0) {
            RaycastHit hit;
            float distance = 3f; //how far the ray shoots
            int layerMask = 1 << NodeLayer;
            //layerMask = ~layerMask; //invert layerMask
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayRadius = NodeBehaviour.nodeRadius;
            if (!Physics.SphereCast(ray, rayRadius, out hit, distance, layerMask)) {
                //node is placed correctly
                var currentNode = carriedNode.GetComponent<NodeBehaviour>();
                Debug.Log(currentNode.node.GetType().ToString());

                //Debug.Log(currentNode.node.Type());
                currentNode.node.position = carriedNode.transform.position;
                nodesPlaced.Add(currentNode.node);
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

    public List<SpellNode> getSpellNodes () {
        return nodesPlaced;
    }

    public bool initiate() {
        return true;
    }
}
