using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class TowerPreview : MonoBehaviour
{
  [Tooltip("Слои, в которых любые коллайдеры блокируют постройку")]
  public LayerMask blockingLayers;

  private SpriteRenderer spriteRenderer;
  private Collider2D coll;

  private bool canPlace = true;
  private int overlapCount = 0;

  // Из BuildingManager смотрит, можно ли строить
  public bool CanPlace => canPlace;

  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    coll = GetComponent<Collider2D>();
    coll.isTrigger = true;
    UpdateColor();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & blockingLayers) != 0)
    {
      overlapCount++;
      canPlace = false;
      UpdateColor();
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & blockingLayers) != 0)
    {
      overlapCount--;
      if (overlapCount <= 0)
      {
        overlapCount = 0;
        canPlace = true;
        UpdateColor();
      }
    }
  }

  private void UpdateColor()
  {
    // зелёный полупрозрачный или красный полупрозрачный
    spriteRenderer.color = canPlace
        ? new Color(0f, 1f, 0f, 0.5f)
        : new Color(1f, 0f, 0f, 0.5f);
  }
}
