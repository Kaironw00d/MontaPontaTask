using System;
using GridSystem;
using UnityEngine;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "Buildings Database", menuName = "Custom/Buildings Database", order = 0)]
    public class BuildingsDatabase : ScriptableObject
    {
        public Building[] buildings;

        public void Init()
        {
            for (var i = 0; i < buildings.Length; i++)
            {
                var building = buildings[i];
                building.Pool = new GameObjectPool(building.gridElement.gameObject);
            }
        }
        
        public bool Get(BuildingType type, out Building building)
        {
            building = null;
            for (var i = 0; i < buildings.Length; i++)
            {
                if (type != buildings[i].type) continue;
                building = buildings[i];
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class Building
    {
        public BuildingType type;
        public GridElement gridElement;
        public GameObjectPool Pool;
    }

    public enum BuildingType
    {
        None,
        Tree
    }
}