using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    [SerializeField] private float superAttackDamage;
    public float AtackSpeed;
    public float AttackRange = 2;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float superAttackCooldownTime;
    public float superAttackTimer { get; private set; } = 0;
    private bool superAttackCharged;

    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;

    private void Update()
    {
        if (Hp <= 0)
        {
            Die();
            return;
        }

        if (!isDead)
        {
            #region Movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementVector = new Vector3(horizontalInput, 0, verticalInput) * movementSpeed;
            movementVector += Vector3.ClampMagnitude(movementVector, movementSpeed);
            transform.position += movementVector * Time.deltaTime;
            AnimatorController.SetFloat("Speed", movementVector.magnitude);
            transform.LookAt(transform.position + movementVector * Time.deltaTime);
            #endregion

            #region Attacking
            if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
            {
                Attack("Attack", Damage);
            }

            if (superAttackTimer > 0)
                superAttackTimer -= Time.deltaTime;
            else if (superAttackTimer <= 0)
                superAttackCharged = true;

            if (Input.GetKeyDown(KeyCode.Mouse1) && EnemyInRange() && CanAttack() && superAttackCharged)
            {
                superAttackCharged = false;
                superAttackTimer = superAttackCooldownTime;
                Attack("SuperAttack", superAttackDamage);
            }
            #endregion
        }
        else
            return;
        /*
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
        }*/
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }
    /// <summary>
    /// Checks if animations "sword attack" and "sword double attack" are currently played or queued to play
    /// </summary>
    /// <returns>True if player can attack. False if player can not</returns>
    private bool CanAttack()
    {
        bool stillAttacking = false;
        foreach (AnimatorClipInfo animation in AnimatorController.GetCurrentAnimatorClipInfo(0))
            if (animation.clip.name == "sword attack" || animation.clip.name == "sword double attack")
                stillAttacking = true;
        foreach (AnimatorClipInfo animation in AnimatorController.GetNextAnimatorClipInfo(0))
            if (animation.clip.name == "sword attack" || animation.clip.name == "sword double attack")
                stillAttacking = true;
        if (stillAttacking)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Finds the closest enemy to the player
    /// </summary>
    /// <returns>Enemy of type Enemie or null if the list is empty</returns>
    private Enemie FindClosestEnemy()
    {
        if (SceneManager.Instance.Enemies.Any())
        {
            var enemies = SceneManager.Instance.Enemies;
            Enemie closestEnemy;
            closestEnemy = enemies[0];
            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                float distanceToClosestEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);
                if (distanceToEnemy < distanceToClosestEnemy)
                    closestEnemy = enemy;
            }
            return closestEnemy;
        }
        return null;
    }

    private void Attack(string attackType, float damage)
    {
        AnimatorController.SetTrigger(attackType);
        Enemie enemyToAttack;
        enemyToAttack = FindClosestEnemy();
        if (enemyToAttack != null)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemyToAttack.transform.position);
            if (distanceToEnemy < AttackRange)
            {
                transform.LookAt(enemyToAttack.transform.position);
                enemyToAttack.Hp -= damage;
            }
        }
    }

    public bool EnemyInRange()
    {
        if (SceneManager.Instance.Enemies.Any())
        {
            var enemies = SceneManager.Instance.Enemies;
            foreach (var enemy in enemies)
                if (Vector3.Distance(transform.position, enemy.transform.position) < AttackRange)
                    return true;
        }
        return false;
    }
}
