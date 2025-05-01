using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TowerMovementManager : MonoBehaviour
{
  public float moveSpeed = 3f;
  public LayerMask platformLayer;
  public float snapDistance = 1f;

  private Tower selectedTower;
  private List<Platform> availableTargets = new List<Platform>();

  void Update()
  {
    Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    if (selectedTower == null && Input.GetMouseButtonDown(0))
    {
      var hit = Physics2D.Raycast(wp, Vector2.zero);
      if (hit.collider != null && hit.collider.TryGetComponent<Tower>(out var t) && !t.IsMoving)
      {
        selectedTower = t;
        ShowAvailablePlatforms();
      }
    }
    else if (selectedTower != null && Input.GetMouseButtonDown(0))
    {
      var hit = Physics2D.Raycast(wp, Vector2.zero);
      if (hit.collider != null && hit.collider.TryGetComponent<Platform>(out var p)
          && availableTargets.Contains(p))
      {
        selectedTower.StartMove(p, moveSpeed);
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
    var curPlat = selectedTower.CurrentPlatform;
    if (curPlat == null) return;

    foreach (var rail in curPlat.connectedRails)
    {
      var other = rail.A == curPlat ? rail.B : rail.A;
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
