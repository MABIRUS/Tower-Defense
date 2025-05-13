using System.Collections.Generic;
using UnityEngine;

public class TowerMovementManager : MonoBehaviour
{
  public static TowerMovementManager Instance { get; private set; }

  public float moveSpeed = 3f;
  public LayerMask platformLayer;
  public float snapDistance = 1f;

  private Tower selectedTower;
  private List<Platform> availableTargets = new ();

  private void Awake() => Instance = this;

  void Update()
  {
    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    if (selectedTower == null && Input.GetMouseButtonDown(0))
    {
      var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
      if (hit.collider != null && hit.collider.TryGetComponent<Tower>(out var tower) && !tower.IsMoving)
      {
        selectedTower = tower;
        ShowAvailablePlatforms();
      }
    }
    else if (selectedTower != null && Input.GetMouseButtonDown(0))
    {
      var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
      if (hit.collider != null && hit.collider.TryGetComponent<Platform>(out var platform)
          && availableTargets.Contains(platform))
      {
        selectedTower.StartMove(platform, moveSpeed);
      }
      ClearSelection();
    }
    else if (selectedTower != null && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
    {
      ClearSelection();
    }
  }

  private void ShowAvailablePlatforms()
  {
    var currentPlat = selectedTower.CurrentPlatform;
    if (currentPlat == null) return;

    foreach (var rail in currentPlat.connectedRails)
    {
      var other = rail.firstPlatform == currentPlat ? rail.secondPlatform : rail.firstPlatform;
      if (!other.HasTower)
      {
        availableTargets.Add(other);
        other.ShowHighlight(true);
      }
    }
  }
  private void ClearSelection()
  {
    foreach (var plat in availableTargets)
      plat.ShowHighlight(false);

    availableTargets.Clear();
    selectedTower = null;
  }

}
