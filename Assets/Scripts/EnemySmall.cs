using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : Enemie
{
    protected override void Start()
    {
        Hp /= 2;
        Damage /= 2;
        AttackRange /= 2;
        transform.localScale /= 2;
        base.Start();
    }
}
