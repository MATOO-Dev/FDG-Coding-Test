using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatEntity
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AttackClosestCombatEntity();
    }
}
