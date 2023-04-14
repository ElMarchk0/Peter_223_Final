using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public StructurePrefabWeighted[] housesPrefabs;
    public StructurePrefabWeighted[] specialPrefabs;
    public StructurePrefabWeighted[] bigStructurePrefabs;
    private float[] houseWeights;
    private float[] specialWeights;
    private float[] bigStructureWeights;

    private void Start()
    {
        // Create an array of prefabs that user can select from
        houseWeights = housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        bigStructureWeights = bigStructurePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    // Place a house
    public void PlaceHouse(Vector3Int position)
    {
        
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(houseWeights);
            placementManager.PlaceObjectOnTheMap(position, housesPrefabs[randomIndex].prefab, CellType.Structure);
            // AudioPlayer.instance.PlayPlacementSound();
        }
    }

    // Place a special building
    public void PlaceSpecial(Vector3Int position)
    {
        
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(specialWeights);
            placementManager.PlaceObjectOnTheMap(position, specialPrefabs[randomIndex].prefab, CellType.Structure);
            // AudioPlayer.instance.PlayPlacementSound();
        }
    }

    public void PlaceBigStructure(Vector3Int position)
    {
        int width = 2;
        int height = 2;
        if (CheckBigStructure(position, width, height))
        {
            int randomIndex = GetRandomWeightedIndex(bigStructureWeights);
            placementManager.PlaceObjectOnTheMap(position, bigStructurePrefabs[randomIndex].prefab, CellType.Structure, width, height);
            // AudioPlayer.instance.PlayPlacementSound();
        }
    }

    // Uses the weight of a prefab to generate a structure
    // Prefabs with a greater weight have a higher probability of being picked
    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }

        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if(randomValue >= tempSum && randomValue < tempSum + weights[i])
            {
                return i;
            }
            tempSum += weights[i];
        }
        return 0;
    }

    // Check if a big structure can be placed at a position
    private bool CheckBigStructure(Vector3Int position, int height, int width)
    {
        bool nearRoad = false;
        // Loop through Cells of prefab to check a position is near a road and free of other structures
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                var newPos = position + new Vector3Int(x, 0, z);
                
                if(!DefaultCheck(newPos))
                {
                    return false;
                } 
                if(!nearRoad)
                {
                    nearRoad = RoadCheck(newPos);
                }
            }
        }
        return nearRoad;
    }  

    // Check if a position is free and in bounds of the map
    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        // Check for roads and standard sized buildings
        if (!DefaultCheck(position)) return false; 
        if (!RoadCheck(position)) return false;     
        return true;
    }

    // Check if position is next to road, does not allow player to place structures that do not have road access
    private bool RoadCheck(Vector3Int position)
    {
        if(placementManager.GetNeighbourOfTypeFor(position, CellType.Road).Count <= 0)
        {
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }
    // Check if a standard sized building can be place at the position
    private bool DefaultCheck(Vector3Int position)
    {
        if (!placementManager.CheckIfPositionInBound(position))
        {
            Debug.Log("This position is out of bounds");
            return false;
        }
        if (!placementManager.CheckIfPositionIsFree(position))
        {
            Debug.Log("This position is not EMPTY");
            return false;
        }
        return true;
    }
}


// Create a prefab and set the weight to randomize the prefabs, 
// prefabs that have been a assigned a higher weight have a greater chance of being created
[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    // Set the weight between 0 and 1
    [Range(0,1)]
    public float weight;
}