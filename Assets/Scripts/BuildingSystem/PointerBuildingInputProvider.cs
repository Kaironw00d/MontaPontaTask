using System;
using System.Collections;
using UnityEngine;

namespace BuildingSystem
{
    public class PointerBuildingInputProvider : MonoBehaviour, IBuildingInputProvider
    {
        public event Action<Vector3> PositionCalculatedEvent;
        
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask layer;

        private void Awake()
        {
            if (cam == null)
                cam = Camera.main;
        }

        public void CalculatePosition()
        {
            StartCoroutine(GetPositionFromPointer());
        }

        private IEnumerator GetPositionFromPointer()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layer))
                    {
                        PositionCalculatedEvent?.Invoke(hitInfo.point);
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }
}