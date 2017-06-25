using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class SpellNode {
    public SpellNode () {
        this.links = new List<Link>();
    }
    public Vector3 position;
    public List<Link> links;
    public uint id;
    public static readonly Dictionary<string, uint> typeToId = new Dictionary<string, uint> { { "SpellNode", 0 }, { "EnergyNode", 1 }, { "HeaterNode", 2 }, { "AimNode", 3 } };
}

public class EnergyNode : SpellNode {
    public EnergyNode() : base() {}
   // public const float standartCapacity = 10f;
    //protected float energyCapacity;
}

public class HeaterNode : EnergyNode {
    public HeaterNode() : base() {}
    //public const float standartSpeed = 10f;
    //float emittingSpeed;
}

public class AimNode : EnergyNode {
    public AimNode() : base() {}
    //public const float standartForce = 10f;
    //float moveForce;
}


