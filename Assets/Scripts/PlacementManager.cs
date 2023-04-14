using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;

    private Dictionary<Vector3Int, StructureModel> temporaryRoadObjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    Grid placementGrid;

    // Check if selected position to build an object is in bounds
    public bool CheckIfPositionInBound(Vector3Int position)
    {
        return position.x >= 0 && position.x < width && position.z >= 0 && position.z < height;
    }

    // Check if there is an exiting structure at the specified position
    public bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfCellType(position, CellType.Empty);
    }

    // Check the type of the cell
    private bool CheckIfPositionIsOfCellType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }

    // Place a building or road on the map
    public void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type, int width = 1, int height = 1)
    {
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                var newPos = position + new Vector3Int(x, 0, z);
                placementGrid[newPos.x, newPos.z] = type;
                structureDictionary.Add(newPos, structure);
                DestroyNatureAt(newPos);
            }
        }        
    }

    // Remove trees and rocks from the map when a structure is created
    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach (var item in hits)
        {
            Destroy(item.collider.gameObject);
        }
    }

    // Build a temporary structure
    public void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadObjects.Add(position, structure);
    }
    public List<Vector3Int>GetNeighbourOfTypeFor(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));            
        }
        return neighbours;
    }

    // Use the StructureModel class to create new structure and return it
    private StructureModel CreateANewStructureModel(Vector3 position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    // Replace one structure with another, used to correct road prefabs
    public void ModifyStructureModel(Vector3Int position,  GameObject newModel, Quaternion rotation)
    {
        if(temporaryRoadObjects.ContainsKey(position))
        {
            temporaryRoadObjects[position].SwapModel(newModel, rotation);
        } 
        else if (structureDictionary.ContainsKey(position))
        {
            structureDictionary[position].SwapModel(newModel, rotation);
        }
    }

    // Get cell types of adjacent cells
    public CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z); 
    }

    // Get a path for a road using A Star search algorithm while using mousedown and drag
    public List<Vector3Int> GetPathsBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach(Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    // Remove all temporary structures while building roads, 
    // used to change the prefab of the road to match the roads around it
    public void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadObjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadObjects.Clear();
    }

    // Transfer the temporary structures to dictionary to save them while building 
    // roads so that path is saved
    public void AddTemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadObjects)
        {
            structureDictionary.Add(structure.Key, structure.Value);
            // DestroyNatureAt(structure.Key);
        }
        temporaryRoadObjects.Clear();
    }


    private void Start()
    {
        placementGrid = new Grid(width, height);
    }
}
