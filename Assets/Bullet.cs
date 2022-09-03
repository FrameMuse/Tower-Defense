using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  private SpriteRenderer spriteRenderer;
  public int damage = 5;
  public GameObject target;
  public float projectileSpeed = 7f;

  void Awake()
  {
    spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

    transform.localScale = new Vector3(0.3f, 0.3f);
  }

  void Start()
  {
    Sprite sprite = Resources.Load<Sprite>("sex");

    spriteRenderer.sprite = sprite;
  }
  float time = 5f;
  void Update()
  {
    time -= Time.deltaTime;
    if (time < 0)
    {
      Destroy(gameObject);
      return;
    }

    if (target == null)
    {
      MoveFallback();
      return;
    }

    Move();
    Rotate();
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

  void Move()
  {
    float step = projectileSpeed * Time.deltaTime;

    Vector3 vectorAZD = Vector3.MoveTowards(transform.position, target.transform.position, step);

    transform.position = vectorAZD;
  }

  void MoveFallback()
  {
    transform.position += transform.right * Time.deltaTime * projectileSpeed;
  }

  void Rotate()
  {
    Vector3 targetPosition = target.transform.position;

    targetPosition.x = targetPosition.x - transform.position.x;
    targetPosition.y = targetPosition.y - transform.position.y;

    float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
  }
}
