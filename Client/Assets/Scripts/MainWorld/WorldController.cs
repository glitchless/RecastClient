using System;
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
        byte[] I_NEED_UPDATES = { 10 };
        Networking.send_udp(Networking.DEFAULT_SERVER_HOSTNAME, Networking.DEFAULT_PORT_UDP, I_NEED_UPDATES, 1);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1))
        {
            var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
            if (hit != null && hit.collider != null)
            {
                var entity = GameObject.Find(hit.collider.gameObject.name);
                if (entity != null)
                {
                    var sprite = entity.GetComponent<SpriteRenderer>();
                    if (sprite != null)
                    {
                        if (selectedObject != null)
                        {
                            selectedObject.GetComponent<SpriteRenderer>().color = naturalColor;
                        }
                        naturalColor = sprite.color;
                        sprite.color = Color.green;
                        selectedObject = entity;
                    }
                }
            }
        }
        
        //var serverData = Networking.recv_udp();
        //var entity_array = Networking.Parse(serverData);
	}
    
    public void ping () {

    }
}

enum EntityType
{
    UNKN = 3,
    MOB = 0,
    FIREBALL = 1,
    SPELL = 2
};

public struct Entity {
    EntityType type;
    uint uniqueId;
    float x;
    float y;

}