using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshUpdate : MonoBehaviour
{

    NavMeshSurface[] _surfaces;

    // Start is called before the first frame update
    void Start()
    {
        _surfaces = GetComponents<NavMeshSurface>();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var surface in _surfaces)
        {
            if (surface.navMeshData != null)
            {
                surface.UpdateNavMesh(surface.navMeshData);
            } else
            {
                surface.BuildNavMesh();
            }
        }
    }
}
