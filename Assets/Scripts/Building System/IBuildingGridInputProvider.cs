using System;
using System.Collections;
using UnityEngine;

public interface IBuildingGridInputProvider
{
    event Action<Vector3> OnPerformedPointerAction;
    IEnumerator TrackPerformedPointerPosition();
}