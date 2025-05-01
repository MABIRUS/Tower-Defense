using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class TowerPreview : MonoBehaviour
{
  [Tooltip("Слой Platforms")]
  public LayerMask platformLayer;

  private SpriteRenderer sr;
  public bool CanPlace { get; private set; }

  void Awake()
  {
    sr = GetComponent<SpriteRenderer>();
    var rb = GetComponent<Rigidbody2D>();
    rb.bodyType = RigidbodyType2D.Kinematic;
    rb.gravityScale = 0;
    var col = GetComponent<Collider2D>();
    col.isTrigger = true;

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
    sr.color = CanPlace
        ? new Color(0f, 1f, 0f, 0.5f)
        : new Color(1f, 0f, 0f, 0.5f);
  }
}
