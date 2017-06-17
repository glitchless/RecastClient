using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class SpellNode {

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

