using UnityEngine;

namespace GridSystem
{
    public class Grid : MonoBehaviour
    {
        [Header("Main")]
        public Vector2Int gridSize = new Vector2Int(30, 30);
        public GridActiveArea[] activeAreas;

        [Header("Gizmos")] 
        [SerializeField] private GizmosMode gizmosMode;
        [SerializeField] private Color activeCellColor = new Color(0f, 0.62f, 1f, 0.59f);
        [SerializeField] private Color inactiveCellColor = new Color(0.5f, 0.5f, 0.5f, 0.59f);

        private GridCell[,] _grid;
        private Vector3Int _gridOffset;

        private void Awake()
        {
            Init();
            SetActiveAreas();
        }

        private void Init()
        {
            var pos = transform.position;
            _gridOffset = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
            _grid = new GridCell[gridSize.x, gridSize.y];
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    _grid[x, y] = new GridCell();
                }
            }
        }

        public bool AddElement(IGridElement element, Vector3 position)
        {
            var intPos = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
            intPos -= _gridOffset;
            if (!CanAddElement(element, intPos)) return false;
            for (var x = 0; x < element.Size.x; x++)
            {
                for (var y = 0; y < element.Size.y; y++)
                {
                    _grid[intPos.x + x, intPos.z + y].isTaken = true;
                }
            }
            
            return true;
        }

        public bool RemoveElement(IGridElement element, Vector3 position)
        {
            var intPos = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
            intPos -= _gridOffset;
            if (!CanRemoveElement(element, intPos)) return false;
            for (var x = 0; x < element.Size.x; x++)
            {
                for (var y = 0; y < element.Size.y; y++)
                {
                    _grid[intPos.x + x, intPos.z + y].isTaken = false;
                }
            }

            return true;
        }

        private bool CanAddElement(IGridElement element, Vector3Int position)
        {
            return InGrid(element, position) && HasSpace(element, position);
        }

        private bool CanRemoveElement(IGridElement element, Vector3Int position)
        {
            return InGrid(element, position) && !HasSpace(element, position);
        }
        
        private bool InGrid(IGridElement element, Vector3Int position)
        {
            return (position.x > 0 || position.x < gridSize.x - element.Size.x) &&
                   (position.y > 0 || position.y < gridSize.y - element.Size.y);
        }
        
        private bool HasSpace(IGridElement element, Vector3Int position)
        {
            for (var x = 0; x < element.Size.x; x++)
            {
                for (var y = 0; y < element.Size.y; y++)
                {
                    var gridCell = _grid[position.x + x, position.z + y];
                    if (!gridCell.isActive || gridCell.isTaken) return false;
                }
            }
            
            return true;
        }
        
        private void SetActiveAreas()
        {
            for (var i = 0; i < activeAreas.Length; i++)
            {
                var activeArea = activeAreas[i];
                if (activeArea.vertex1 == activeArea.vertex2)
                {
                    _grid[activeArea.vertex1.x, activeArea.vertex1.y].isActive = true;
                    return;
                }
                
                for (var x = activeArea.vertex1.x; x < activeArea.vertex2.x; x++)
                {
                    for (var y = activeArea.vertex1.y; y < activeArea.vertex2.y; y++)
                    {
                        _grid[x, y].isActive = true;
                    }
                }
            }
        }

        private void OnValidate()
        {
            Init();
            SetActiveAreas();
        }

        private void OnDrawGizmosSelected()
        {
            if(gizmosMode != GizmosMode.SelectedOnly) return;
            DrawGizmos();
        }

        private void OnDrawGizmos()
        {
            if(gizmosMode != GizmosMode.Always) return;
            DrawGizmos();
        }

        private void DrawGizmos()
        {
            if (_grid == null)
                Init();

            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    var pos = transform.position + new Vector3(x, 0, y);
                    pos.y = 1;
                    Gizmos.color = _grid[x,y].isActive ? activeCellColor : inactiveCellColor;
                    Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
                }
            }
        }
    }
}