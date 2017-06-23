using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Point {
    Point(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    float x;
    float y;
    float z;
}

public class Link: System.IEquatable<Link> {
    public Link (Vector3 start) {
        this.start = start;
    }
    public Link(Vector3 start, Vector3 end) {
        this.start = start;
        this.end = end;
    }
    public Vector3 start;
    public Vector3 end;
    public Link reverse () {
        return new Link(this.end, this.start);
    }

    public bool Equals(Link other)
    {
        return (this.start == other.start && this.end == other.end);
    }
}

abstract public class SpellNode {
    public List<Link> links;
}

public class EnergyNode : SpellNode {
    public EnergyNode(float energyCapacity) {
        this.energyCapacity = energyCapacity;
    }

    public static readonly float standartCapacity = 10f;
    float energyCapacity;
}

public class EmitterNode : SpellNode {
    public EmitterNode(float emittingSpeed) {
        this.emittingSpeed = emittingSpeed;
    }

    public static readonly float standartSpeed = 10f;
    float emittingSpeed;
}

