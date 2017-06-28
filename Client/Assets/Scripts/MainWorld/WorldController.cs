using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : Singleton<WorldController> {
    protected WorldController() {}
    GameObject selectedObject;
    Color naturalColor;
	// Use this for initialization
	void Start () {
        naturalColor = Color.clear;
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
            if (hit != null && hit.collider != null) {
                var entity = GameObject.Find(hit.collider.gameObject.name);
                if (entity != null) {
                    var sprite = entity.GetComponent<SpriteRenderer>();
                    if (sprite != null) {
                        if (selectedObject != null) {
                            selectedObject.GetComponent<SpriteRenderer>().color = naturalColor;
                        }
                        naturalColor = sprite.color;
                        sprite.color = Color.green;
                        selectedObject = entity;
                    }
                }
            }
        }
	}
    
    public void ping () {

    }
}
