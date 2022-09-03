using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public GameObject target;
  private Vector3 targetLastPosition;
  public ProjectileType type = ProjectileType.Following;

  public int damage = 5;
  public float moveSpeed = 7f;
  public float lifetime = 5f;

  private SpriteRenderer spriteRenderer;

  void Start()
  {
    StartCoroutine("DestroyLifetime");


    transform.localScale = new Vector3(0.3f, 0.3f);

    spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    Sprite sprite = Resources.Load<Sprite>("sex");
    spriteRenderer.sprite = sprite;
  }
  void Update()
  {
    if (target == null)
    {
      MoveForward();

      return;
    }

    MoveTowards();
    ApplyDamage();
  }
  void ApplyDamage()
  {
    if (Vector3.Distance(transform.position, target.transform.position) <= 0.01f)
    {
      Destroy(gameObject);

      if (target.TryGetComponent(out IDamageable hit))
      {
        hit.ApplyDamage(damage);
      }
    }
  }

  void MoveTowards()
  {
    float moveStep = moveSpeed * Time.deltaTime;
    targetLastPosition = target.transform.position;

    Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetLastPosition, moveStep);
    transform.position = nextPosition;
  }

  void MoveForward()
  {
    float moveStep = moveSpeed * Time.deltaTime;
    Vector3 directionDelta = targetLastPosition - transform.position;

    transform.position += targetLastPosition.normalized * moveStep;
  }

  IEnumerator DestroyLifetime()
  {
    yield return new WaitForSeconds(lifetime);

    Destroy(gameObject);
  }
}