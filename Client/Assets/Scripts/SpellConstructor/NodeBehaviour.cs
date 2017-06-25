using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class NodeBehaviour : MonoBehaviour {
    public SpellNode node;
    public static readonly float nodeRadius = 0.5f;
    Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 1);
    Material material;

    void Start () {
        node = new EnergyNode();
        node.links = new List<Link>();
        this.transform.position = Input.mousePosition;
        var meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }

    private void OnMouseDown() {
        switch (Controller.Instance.state) {
            case Controller.status.none:
                Debug.Log("Linking Begins");
                Controller.Instance.state = Controller.status.linkingStart;
                Controller.Instance.currentLink = new Link(this.transform.position);
                Controller.Instance.linkBeginning = this.gameObject;
                break;
            case Controller.status.linkOnTarget:
                Debug.Log("Linked Successfully");
                Controller.Instance.state = Controller.status.linked;
                break;
        }
    }

    private void OnMouseEnter() {
        Debug.Log("MouseEnter");
        material.color = Color.green;
        if (Controller.Instance.state == Controller.status.linkingNodes) {
            Controller.Instance.state = Controller.status.linkOnTarget;
            Controller.Instance.linkTarget = this.gameObject;
        }
    }

    private void OnMouseExit() {
        Debug.Log("MouseExit");
        if (Controller.Instance.state == Controller.status.linkOnTarget) {
            Controller.Instance.state = Controller.status.linkingNodes;
        }
        material.color = defaultColor;
    }

    void Update () {
		
	}
}
