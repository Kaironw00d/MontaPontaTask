using System;
using UnityEngine;
using Color = UnityEngine.Color;

namespace GridSystem
{
    public enum GizmosMode
    {
        Off,
        SelectedOnly,
        Always
    }
    
    public class GridElement : MonoBehaviour, IGridElement
    {
        public event Action<IGridElement, Vector3> OnBuildingDisable;
        
        [Header("Main"), SerializeField] private Vector2Int size;
        public Vector2Int Size => size;

        [Header("Gizmos")] 
        [SerializeField] private GizmosMode gizmosMode;
        [SerializeField] private Color gizmosColor = new Color(1f, 0.48f, 0f, 0.59f);

        private void OnDisable() => OnBuildingDisable?.Invoke(this, transform.position);

        private void OnDrawGizmosSelected()
        {
            if (gizmosMode != GizmosMode.SelectedOnly) return;
            DrawGizmos();
        }
        
        private void OnDrawGizmos()
        {
            if(gizmosMode != GizmosMode.Always) return;
            DrawGizmos();
        }
        
        private void DrawGizmos()
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var pos = transform.position + new Vector3(x, 0, y);
                    pos.y = 1;
                    Gizmos.color = gizmosColor;
                    Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
                }
            }
        }
    }
}