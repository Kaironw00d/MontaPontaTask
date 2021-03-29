using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Custom/Buildings", order = 0)]
public class BuildingsDatabase : ScriptableObject
{
    public Building[] buildings;

    public bool Get(BuildingType type, out Building building)
    {
        building = null;
        for (var i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].type == type)
            {
                building = buildings[i];
                return true;
            }
        }
        return false;
    }
}

public enum BuildingType
{
    Tree
}

[System.Serializable]
public class Building
{
    public BuildingType type;
    public BuildingGridElement building;
    public GameObjectPool pool;
}