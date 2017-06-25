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

    public uint endId;

    public Link reverse () {
        return new Link(this.end, this.start);
    }

    public bool Equals(Link other)
    {
        return (this.start == other.start && this.end == other.end);
    }
}

abstract public class SpellNode {
    public SpellNode () {}
    public List<Link> links;
    public uint id;
    public enum types {
        commonNode,
        energyNode,
        heaterNode,
        aimNode
    }

    public types type = types.commonNode;
}

public class EnergyNode : SpellNode {
    public EnergyNode() {}
    public new types type = types.energyNode;
   // public const float standartCapacity = 10f;
    //protected float energyCapacity;
}

public class HeaterNode : EnergyNode {
    public HeaterNode() {}
    public new types type = types.heaterNode;
    //public const float standartSpeed = 10f;
    //float emittingSpeed;
}

public class AimNode : EnergyNode {
    public AimNode() {}
    public new types type = types.aimNode;
    //public const float standartForce = 10f;
    //float moveForce;
}


