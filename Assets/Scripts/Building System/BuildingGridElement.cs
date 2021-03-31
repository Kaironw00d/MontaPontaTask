using System;
using UnityEngine;

public class BuildingGridElement: MonoBehaviour
{
    public event Action<BuildingGridElement> OnBuildingDisable;
    public Vector2Int size = Vector2Int.one;
    public BuildingType type;

    private void OnDisable() => OnBuildingDisable?.Invoke(this);

    private void OnDrawGizmosSelected()
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = transform.position + new Vector3(x, 0, y);
                pos.y = 0;
                Gizmos.color = new Color(1f, 0f, 0f, 0.51f);
                Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
            }
        }
    }
}
