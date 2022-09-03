using cakeslice;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{

  public int health = 100;
  public float moveSpeed = 1f;

  public Outline outline;

  void Start()
  {
    health = 100;

    outline = gameObject.AddComponent<Outline>();
    outline.color = 0;
  }

  void Update()
  {
    Move();
  }

  void Move()
  {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    float multiplier = Time.deltaTime * moveSpeed;

    transform.position += new Vector3(horizontal, vertical) * multiplier;
  }

  public void ApplyDamage(int damage)
  {
    health -= damage;

    if (health <= 0)
    {
      Destroy(gameObject);
    }
  }
}
