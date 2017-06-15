using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class SpellNode {
    protected SpellNode(float coordX, float coordY, float coordZ) {
        location = new Point(coordX, coordY, coordZ);
    }

    public Point location;

    public class Point {
        public Point(float coordX, float coordY, float coordZ) {
            x = coordX;
            y = coordY;
            z = coordZ;
        }

        float x;
        float y;
        float z;
    }
}

public class EnergyNode : SpellNode {
    public EnergyNode(float coordX, float coordY, float coordZ, float energyCapacity) : base(coordX, coordY, coordZ) {
        this.energyCapacity = energyCapacity;
    }

    float energyCapacity;
}

public class EmitterNode : SpellNode {
    public EmitterNode(float coordX, float coordY, float coordZ, float emittingSpeed) : base(coordX, coordY, coordZ) {
        this.emittingSpeed = emittingSpeed;
    }

    float emittingSpeed;
}

