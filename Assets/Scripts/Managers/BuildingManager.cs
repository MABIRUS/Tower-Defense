using UnityEngine;

public class BuildingManager : MonoBehaviour
{
  public static BuildingManager Instance { get; private set; }
  public PlayerBase playerBase;
  public LayerMask platformLayer;
  public float snapDistance = 1f;

  ShopItemSO current;
  GameObject previewGO;
  Platform railStart;
  TowerPreview towerPreview;

  void Awake() => Instance = this;

  void Update()
  {
    if (previewGO == null) return;

    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    switch (current.buildType)
    {
      case BuildType.Tower:
        previewGO.transform.position = pos;
        var np = FindNearestPlatform(pos);
        if (np != null &&
            Vector2.Distance(pos, np.transform.position) < snapDistance)
        {
          previewGO.transform.position = np.transform.position;
        }
        break;

      case BuildType.Platform:
        previewGO.transform.position = pos;
        var nP = FindNearestPlatform(pos);
        if (nP != null &&
            Vector2.Distance(pos, nP.transform.position) < snapDistance)
        {
          previewGO.transform.position = nP.transform.position;
        }
        break;

      case BuildType.Rail:
        if (railStart == null)
        {
          if (Input.GetMouseButtonDown(0))
          {
            var platform1 = FindNearestPlatform(pos);
            if (platform1 != null) railStart = platform1;
          }
        }
        else
        {
          var platform2 = FindNearestPlatform(pos);
          var a = railStart;
          var b = platform2 != null ? platform2 : railStart;
          previewGO.GetComponent<Rail>().Initialize(a, b);
        }
        break;
    }

    if (Input.GetMouseButtonDown(0))
    {
      if (playerBase.Money >= current.price)
      {
        switch (current.buildType)
        {
          case BuildType.Tower:
            if (towerPreview.CanPlace)
            {
              var tower = Instantiate(current.buildPrefab, previewGO.transform.position, Quaternion.identity)
                .GetComponent<Tower>();
              playerBase.ChangeMoney(-current.price);
              var plat = FindNearestPlatform(previewGO.transform.position);
              if (plat != null)
              {
                plat.OccupiedTower = tower;
                tower.CurrentPlatform = plat;
              }
            }
            break;

          case BuildType.Platform:
            Instantiate(current.buildPrefab,
                        previewGO.transform.position,
                        Quaternion.identity);
            playerBase.ChangeMoney(-current.price);
            break;

          case BuildType.Rail:
            var r = previewGO.GetComponent<Rail>();
            Instantiate(current.buildPrefab)
                .GetComponent<Rail>()
                .Initialize(r.firstPlatform, r.secondPlatform);
            playerBase.ChangeMoney(-current.price);
            break;
        }
      }
      EndPlacement();
    }

    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
      EndPlacement();
  }

  public void BeginPlacement(ShopItemSO item)
  {
    EndPlacement();
    current = item;
    previewGO = Instantiate(current.previewPrefab);
    railStart = null;
    if (current.buildType == BuildType.Tower)
      towerPreview = previewGO.GetComponent<TowerPreview>();
  }

  void EndPlacement()
  {
    if (previewGO) Destroy(previewGO);
    previewGO = null;
    current = null;
    railStart = null;
    towerPreview = null;
  }

  Platform FindNearestPlatform(Vector2 pos)
  {
    Platform best = null;
    var md = snapDistance;
    foreach (var platform in Object.FindObjectsByType<Platform>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
    {
      var distance = Vector2.Distance(pos, platform.transform.position);
      if (distance < md)
      {
        md = distance;
        best = platform;
      }
    }
    return best;
  }
}
