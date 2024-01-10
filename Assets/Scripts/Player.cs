using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    [SerializeField] private float movementSpeed;

    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementVector = new Vector3 (horizontalInput, 0, verticalInput) * movementSpeed;
        movementVector += Vector3.ClampMagnitude(movementVector, movementSpeed);
        transform.position += movementVector * Time.deltaTime;
        transform.LookAt(movementVector);
        AnimatorController.SetFloat("Speed", movementVector.magnitude);

        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }


        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemie.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= AttackRange)
            {
                if (Time.time - lastAttackTime > AtackSpeed)
                {
                    //transform.LookAt(closestEnemie.transform);
                    transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                    lastAttackTime = Time.time;
                    closestEnemie.Hp -= Damage;
                    AnimatorController.SetTrigger("Attack");
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }


}
