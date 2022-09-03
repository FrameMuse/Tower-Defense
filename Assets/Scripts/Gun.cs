using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
  [SerializeField]
  private GameObject target;

  public int attackDamage = 1;
  public float attackCoolDown = 0.2f;
  public AttackTargetType attackTargetType = AttackTargetType.Nearest;


  public float reachDistance = 5f;
  private List<Projectile> projectiles = new();

  private void Start()
  {
    StartCoroutine("AttackRoutine");
  }

  private void Update()
  {
    Enemy[] enemies = FindObjectsOfType<Enemy>();
    Enemy[] reachableEnemies = enemies.Where(enemy => IsReachable(enemy.gameObject)).ToArray();

    GameObject[] reachableEnemyGameObjects = reachableEnemies.Select(enemy => enemy.gameObject).ToArray();

    if (reachableEnemies.Length == 0 || Array.IndexOf(reachableEnemyGameObjects, target) == -1)
    {
      target = null;
    }

    LockNearestTarget(reachableEnemyGameObjects);
    RemoveAllUnreachableProjectilesTarget(enemies.Select(enemy => enemy.gameObject).ToArray());


    foreach (Enemy enemy in enemies) enemy.outline.enabled = false;
    if (target)
    {
      enemies.Where(enemy => enemy.gameObject == target).First().outline.enabled = true;
    }
  }

  private void Attack()
  {
    if (target == null) return;

    CreateProjectile(target, transform);
  }

  private void RemoveProjectilesTarget(GameObject withTarget)
  {
    foreach (Projectile projectile in projectiles)
    {
      if (projectile == null) continue;
      if (projectile.target != withTarget) continue;

      projectile.target = null;
    }
  }

  void LockNearestTarget(GameObject[] targets)
  {
    if (target != null) return;

    GameObject nearestTarget = Targeting.GetNearest(targets, transform.position);
    target = nearestTarget;
  }

  void RemoveAllUnreachableProjectilesTarget(GameObject[] targets)
  {
    foreach (GameObject target in targets)
    {
      if (IsReachable(target)) return;

      RemoveProjectilesTarget(target);
    }
  }

  bool IsReachable(GameObject target)
  {
    float distance = Vector3.Distance(target.transform.position, transform.position);

    return distance <= reachDistance;
  }

  private Projectile CreateProjectile(GameObject target)
  {
    GameObject gameObject = new GameObject("Projectile");
    Projectile projectile = gameObject.AddComponent<Projectile>();

    projectile.transform.position = transform.position;
    projectile.target = target;
    projectile.damage = attackDamage;
    projectile.enabled = true;

    projectiles.Add(projectile);

    return projectile;
  }
  private Projectile CreateProjectile(GameObject target, Transform parent)
  {
    Projectile projectile = CreateProjectile(target);
    projectile.transform.parent = parent;

    return projectile;
  }

  private IEnumerator AttackRoutine()
  {
    while (true)
    {
      Attack();
      yield return new WaitForSeconds(attackCoolDown);
    }
  }

  private void OnDrawGizmos()
  {
    if (target == null) return;

    float distance = Vector3.Distance(target.transform.position, transform.position);
    if (distance > reachDistance) return;

    Gizmos.DrawLine(transform.position, target.transform.position);
  }
}