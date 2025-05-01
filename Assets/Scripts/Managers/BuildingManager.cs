using UnityEngine;

public class BuildingManager : MonoBehaviour
{
  public static BuildingManager Instance;
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
        // свободно за курсором
        previewGO.transform.position = pos;
        // «магнит» к платформе
        var np = FindNearestPlatform(pos);
        if (np != null &&
            Vector2.Distance(pos, np.transform.position) < snapDistance)
        {
          previewGO.transform.position = np.transform.position;
        }
        break;

      case BuildType.Platform:
        previewGO.transform.position = pos;
        var nn = FindNearestPlatform(pos);
        if (nn != null &&
            Vector2.Distance(pos, nn.transform.position) < snapDistance)
        {
          previewGO.transform.position = nn.transform.position;
        }
        break;

      case BuildType.Rail:
        if (railStart == null)
        {
          if (Input.GetMouseButtonDown(0))
          {
            var p = FindNearestPlatform(pos);
            if (p != null) railStart = p;
          }
        }
        else
        {
          var p2 = FindNearestPlatform(pos);
          var a = railStart;
          var b = p2 ?? railStart;
          previewGO.GetComponent<Rail>().Initialize(a, b);
        }
        break;
    }

    // Построить по ЛКМ
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
              // «занять» платформу и дать башне ссылку
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
                .Initialize(r.A, r.B);
            playerBase.ChangeMoney(-current.price);
            break;
        }
      }
      EndPlacement();
    }

    // Отмена по ПКМ или Esc
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
    float md = snapDistance;
    foreach (var p in Object.FindObjectsByType<Platform>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
    {
      float d = Vector2.Distance(pos, p.transform.position);
      if (d < md)
      {
        md = d;
        best = p;
      }
    }
    return best;
  }
}
