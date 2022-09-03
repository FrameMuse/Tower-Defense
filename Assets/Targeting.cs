using UnityEngine;

public static class Targeting
{
  static public GameObject GetNearest(GameObject[] targets, Vector3 position)
  {
    float nearestDistance = Mathf.Infinity;
    GameObject nearestTarget = null;

    foreach (GameObject target in targets)
    {
      float distance = Vector3.Distance(target.transform.position, position);

      if (distance < nearestDistance)
      {
        nearestDistance = distance;
        nearestTarget = target;
      }
    }

    return nearestTarget;
  }

}