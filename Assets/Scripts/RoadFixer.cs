using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadFixer : MonoBehaviour
{
    //Road prefabs
    public GameObject deadEnd;
    public GameObject roadStraight;
    public GameObject corner;
    public GameObject threeWay;
    public GameObject fourWay;

    // fix the roads position based on the roads around itself, IE create a dead end if you 
    // creating one cell of road, or create a continuos road if there is an adjacent road cell
    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int tempPosition)
    {
        // Get then cell types of the neighbours of the position in of right, up, left, down
        var result = placementManager.GetNeighbourTypesFor(tempPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        if(roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(placementManager, result, tempPosition);
        } 
        else if (roadCount == 2)
        {
            if(CreateStraightRoad(placementManager, result, tempPosition))
            {
                return;
            }
            CreateCorner(placementManager, result, tempPosition);
        }
        else if(roadCount == 3)
        {
            CreateThreeWay(placementManager, result, tempPosition);
        }
        else if(roadCount == 4)
        {
            CreateFourWay(placementManager, result, tempPosition);
        }
    }

    // Create a continuos straight road, checks orientation of adjacent roads
    // If false is returned then a corner is created
    private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
    {
        if(result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, roadStraight, Quaternion.identity);
            return true; 
        } 
        else if(result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, roadStraight, Quaternion.Euler(0,90,0));
            return true;
        }
        return false;
    }

    // Create a dead end
    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
    {
        if(result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, deadEnd, Quaternion.Euler(0,180,0));
        } 
        else if (result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, deadEnd, Quaternion.Euler(0,270,0));
        }
        else if (result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, deadEnd, Quaternion.identity);
        }
        else if (result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, deadEnd, Quaternion.Euler(0,90,0));
        }
    }

    // Checks if roads intersect at a 90 angle and creates a corner
    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
    {
        if(result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, corner, Quaternion.Euler(0,90,0));
        } 
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, corner, Quaternion.Euler(0,180,0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, corner, Quaternion.Euler(0,270,0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, corner, Quaternion.Euler(0,0,0));
        }
    }

    // Create a three way intersection, check for an empty cell to determine the orientation of the three way intersection
    // Rotates the prefab if necessary
    private void CreateThreeWay(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
    {
        if(result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, threeWay, Quaternion.identity);
        } 
        else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, threeWay, Quaternion.Euler(0,90,0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, threeWay, Quaternion.Euler(0,180,0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPosition, threeWay, Quaternion.Euler(0,270,0));
        }
    }

    // Create a four way intersection, 
    //the intersection will look the same no matter way so we can just place the prefab
    private void CreateFourWay(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
    {
        placementManager.ModifyStructureModel(tempPosition, fourWay, Quaternion.identity);
    }
}
