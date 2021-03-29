using System;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(30, 30);

    private bool[,] _grid;
    private Vector3 _gridPosition;

    private void Awake()
    {
        _grid = new bool[gridSize.x, gridSize.y];
        _gridPosition = transform.position;
    }

    public bool PlaceBuilding(BuildingGridElement building, Vector3 position)
    {
        position -= _gridPosition;
        if (!InGrid(building, (int) position.x, (int) position.z)) return false;
        if(!HasSpace(building, (int) position.x, (int) position.z)) return false;
        for (var x = 0; x < building.size.x; x++)
        {
            for (var y = 0; y < building.size.y; y++)
            {
                _grid[(int)position.x + x, (int)position.z + y] = true;
            }
        }
        return true;
    }
    
    public bool RemoveBuilding(BuildingGridElement building, Vector3 position)
    {
        position -= _gridPosition;
        if (!InGrid(building, (int) position.x, (int) position.z)) return false;
        if (HasSpace(building, (int) position.x, (int) position.z)) return false;
        for (var x = 0; x < building.size.x; x++)
        {
            for (var y = 0; y < building.size.y; y++)
            {
                _grid[(int)position.x + x, (int)position.z + y] = false;
            }
        }
        return true;
    }
    
    private bool HasSpace(BuildingGridElement building, int posX, int posY)
    {
        for (var x = 0; x < building.size.x; x++)
        {
            for (var y = 0; y < building.size.y; y++)
            {
                if (_grid[posX + x, posY + y]) return false;
            }
        }
        return true;
    }

    private bool InGrid(BuildingGridElement building, int posX, int posY)
    {
        if (posX < 0 || posX > gridSize.x - building.size.x) return false;
        if (posY < 0 || posY > gridSize.y - building.size.y) return false;
        return true;
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if(_grid == null)
            _grid = new bool[gridSize.x, gridSize.y];
        #endif
        
        for (var x = 0; x < gridSize.x; x++)
        {
            for (var y = 0; y < gridSize.y; y++)
            {
                Gizmos.color = _grid[x,y] ? new Color(1f, 0f, 0f, 0.55f) : new Color(0f, 1f, 1f, 0.51f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }
}
