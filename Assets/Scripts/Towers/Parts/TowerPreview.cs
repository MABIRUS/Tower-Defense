using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class TowerPreview : MonoBehaviour
{
  public LayerMask platformLayer;

  private SpriteRenderer spriteRenderer;
  public bool CanPlace { get; private set; }

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();

    var rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.bodyType = RigidbodyType2D.Kinematic;
    rigidbody.gravityScale = 0;

    var collider = GetComponent<Collider2D>();
    collider.isTrigger = true;

    CanPlace = false;
    UpdateColor();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & platformLayer) != 0)
    {
      var plat = other.GetComponent<Platform>();
      CanPlace = plat != null && !plat.HasTower;
      UpdateColor();
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & platformLayer) != 0)
    {
      CanPlace = false;
      UpdateColor();
    }
  }

  void UpdateColor()
  {
    spriteRenderer.color = CanPlace
        ? new Color(0f, 1f, 0f, 0.5f)
        : new Color(1f, 0f, 0f, 0.5f);
  }
}
