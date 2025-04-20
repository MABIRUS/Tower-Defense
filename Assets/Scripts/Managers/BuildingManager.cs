using UnityEngine;

public class BuildingManager : MonoBehaviour
{
  [Header("References")]
  public PlayerBase playerBase;
  private Camera mainCam;

  private GameObject previewInstance;
  private TowerPreview previewScript;
  private ShopItemSO currentItem;

  private void Awake()
  {
    mainCam = Camera.main;
  }

  private void Update()
  {
    if (previewInstance == null) return;

    // Перемещаем призрак за мышью
    Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    previewInstance.transform.position = worldPos;

    // ЛКМ — попытка разместить башню
    if (Input.GetMouseButtonDown(0))
    {
      // Только если можно ставить и есть деньги
      if (previewScript.CanPlace && playerBase.Money >= currentItem.price)
      {
        playerBase.ChangeMoney(-currentItem.price);
        Instantiate(currentItem.towerPrefab, worldPos, Quaternion.identity);
      }
      CancelPlacement();
    }

    // ПКМ или Esc — отмена режима размещения
    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
    {
      CancelPlacement();
    }
  }

  /// <summary>
  /// Начать режим размещения указанного товара.
  /// </summary>
  public void BeginPlacement(ShopItemSO item)
  {
    // Если денег не хватает — сразу выходим
    if (playerBase.Money < item.price)
      return;

    // Отменяем прошлый призрак, если есть
    if (previewInstance != null)
      Destroy(previewInstance);

    currentItem = item;
    previewInstance = Instantiate(item.previewPrefab);
    previewScript = previewInstance.GetComponent<TowerPreview>();
  }

  private void CancelPlacement()
  {
    if (previewInstance != null)
      Destroy(previewInstance);

    previewInstance = null;
    previewScript = null;
    currentItem = null;
  }
}
