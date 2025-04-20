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

    // ���������� ������� �� �����
    Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    previewInstance.transform.position = worldPos;

    // ��� � ������� ���������� �����
    if (Input.GetMouseButtonDown(0))
    {
      // ������ ���� ����� ������� � ���� ������
      if (previewScript.CanPlace && playerBase.Money >= currentItem.price)
      {
        playerBase.ChangeMoney(-currentItem.price);
        Instantiate(currentItem.towerPrefab, worldPos, Quaternion.identity);
      }
      CancelPlacement();
    }

    // ��� ��� Esc � ������ ������ ����������
    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
    {
      CancelPlacement();
    }
  }

  /// <summary>
  /// ������ ����� ���������� ���������� ������.
  /// </summary>
  public void BeginPlacement(ShopItemSO item)
  {
    // ���� ����� �� ������� � ����� �������
    if (playerBase.Money < item.price)
      return;

    // �������� ������� �������, ���� ����
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
