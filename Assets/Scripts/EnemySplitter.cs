using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitter : Enemie
{
    [SerializeField] GameObject smallEnemy;

    protected override void Die()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(smallEnemy, new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5)), smallEnemy.transform.rotation);
        }
        base.Die();
    }
}
