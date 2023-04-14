using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;

    // List of previously place roads in a sequence
    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    // List of positions to check for existing roads to check what prefab show be used when building a road
    public List<Vector3Int> roadPositionsToCheck = new List<Vector3Int>();    

    private Vector3Int startPosition;
    private bool placementMode = false;

    public RoadFixer roadFixer;

    // Populate the temporary placement position list with road positions 
    // and place the appropriate road type prefab at that position clicked on
    public void PlaceRoad(Vector3Int position)
    {
        // Check if position is in bounds
        if (!placementManager.CheckIfPositionInBound(position))
        {
            return;
        }
        // Check if position is free of existing structures
        if (!placementManager.CheckIfPositionIsFree(position))
        {
            return;
        }
        if(!placementMode)
        {
            // Clear the temporary placement position list 
            //and start a new list starting at the new position
            temporaryPlacementPositions.Clear();
            roadPositionsToCheck.Clear();
            placementMode = true;
            startPosition = position;
            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road);
            
        } 
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();
            // Fix existing roads
            foreach (var positionsToFix in roadPositionsToCheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);
            }

            roadPositionsToCheck.Clear();
            // Generate a path for roads to be placed
            temporaryPlacementPositions = placementManager.GetPathsBetween(startPosition, position);
            foreach (var tempPosition in temporaryPlacementPositions)            
            {
                // Check if position is free of existing structures
                if (!placementManager.CheckIfPositionIsFree(tempPosition))
                {
                    roadPositionsToCheck.Add(tempPosition);
                    continue;
                }
                placementManager.PlaceTemporaryStructure(tempPosition, roadFixer.deadEnd, CellType.Road);
            }
        }
        FixRoadPrefabs();
    }

    // Check the neighbouring cells and orientation of existing road so that the correct road type is created
    private void FixRoadPrefabs()
    {
        foreach(var tempPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, tempPosition);
            var neighbours = placementManager.GetNeighbourOfTypeFor(tempPosition, CellType.Road);
            foreach (var roadPosition in neighbours) 
            {
                if(!roadPositionsToCheck.Contains(roadPosition))
                {
                    roadPositionsToCheck.Add(roadPosition);
                }
            }
        }
        foreach(var positionToFix in roadPositionsToCheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }

    // Finishing placing a road and clear temporary positions dictionary
    public void FinishPlacingRoad()
    {
        placementMode = false;
        placementManager.AddTemporaryStructuresToStructureDictionary();
        temporaryPlacementPositions.Clear();
        startPosition = Vector3Int.zero;        
    }

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }
}
