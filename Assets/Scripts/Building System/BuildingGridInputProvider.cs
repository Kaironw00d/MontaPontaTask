using System;
using System.Collections;
using UnityEngine;

public class BuildingGridInputProvider : MonoBehaviour, IBuildingGridInputProvider
{
    public KeyCode keyCode = KeyCode.Mouse0;
    public event Action<Vector3> OnPerformedPointerAction;
    
    public IEnumerator TrackPerformedPointerPosition()
    {
        while (true)
        {
            if (Input.GetKeyDown(keyCode))
            {
                OnPerformedPointerAction?.Invoke(Input.mousePosition);
                yield break;
            }
            yield return null;
        }
    }
}