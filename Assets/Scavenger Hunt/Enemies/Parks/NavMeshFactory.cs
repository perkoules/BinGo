using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Enums;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Mapbox/Factories/NavMesh Factory")]
public class NavMeshFactory : AbstractTileFactory
{
    [SerializeField] private int navMeshSettingsIndex;

    private NavMeshBuildSettings settings;
    private NavMeshData navMeshData;

    private readonly Dictionary<UnityTile, NavMeshBuildSource> buildSources =
        new Dictionary<UnityTile, NavMeshBuildSource>();

    protected override void OnInitialized()
    {
        settings = NavMesh.GetSettingsByIndex(navMeshSettingsIndex);
        navMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
    }

    protected override void OnRegistered(UnityTile tile)
    {
        if (tile.HeightDataState == TilePropertyState.Loading)
        {
            tile.OnVectorDataChanged += UpdateNavMesh;
        }
        else
        {
            UpdateNavMesh(tile);
        }
    }

    protected override void OnUnregistered(UnityTile tile)
    {
        buildSources.Remove(tile);
         UpdateNavMesh(tile);
    }

    private void UpdateNavMesh(UnityTile tile)
    {
        if (buildSources.ContainsKey(tile))
        {
            buildSources.Remove(tile);
        }

        buildSources.Add(tile, new NavMeshBuildSource()
        {
            shape = NavMeshBuildSourceShape.Mesh,
            sourceObject = tile.MeshFilter.mesh,
            transform = tile.transform.localToWorldMatrix,
            area = 0
        });

        NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, settings, new List<NavMeshBuildSource>(buildSources.Values),
            new Bounds(tile.transform.position, Vector3.one * 100000));
    }

    protected override void OnPostProcess(UnityTile tile)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnUnbindEvents()
    {
        throw new System.NotImplementedException();
    }
}