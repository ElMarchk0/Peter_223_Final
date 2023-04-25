using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCity.AI
{
    public class RoadHelperStraight : RoadHelper
    {
        [SerializeField]
        private Marker leftLaneMarker90;
        [SerializeField]
        private Marker rightLaneMarker90;

        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        // Determine if a vehicle is traveling north/south or eat/west
        public Direction GetDirection(Vector3 direction)
        {
            // Determine if a vehicle is traveling North/South
            if(Mathf.Abs(direction.z) > .5f)
                {
                    if(direction.z > 0.5f)
                    {
                        return Direction.up;
                    }
                    else
                    {
                        return Direction.down;
                    }
                }
                // Determine if a vehicle is traveling East/West
                else
                {
                    if(direction.x > 0.5f)
                    {
                        return Direction.right;
                    }
                    else
                    {
                        return Direction.left;
                    }
                }
        }

        // Spawn a vehicle on a section of straight road
        public override Marker GetPositionForCarToSpawn(Vector3 nextPathPosition)
        {
            int angle = (int)transform.rotation.eulerAngles.y;
            var direction = nextPathPosition - transform.position;
            return GetCorrectMarker(angle, direction);
        }

        // De-spawn a vehicle on a section of straight road
        public override Marker GetPositionForCarToEnd(Vector3 previousPathPosition)
        {
            int angle = (int)transform.rotation.eulerAngles.y;
            var direction = transform.position - previousPathPosition;
            return GetCorrectMarker(angle, direction);
        }

        // Determine the lane the car must use to travel
        private Marker GetCorrectMarker(int angle, Vector3 directionVector)
        {
            var direction = GetDirection(directionVector);
            if (angle == 0)
            {
                if (direction == Direction.left)
                {
                    return rightLaneMarker90;
                }
                else
                {
                    return leftLaneMarker90;
                }
            } else if (angle == 90)
            {
                if (direction == Direction.up)
                {
                    return rightLaneMarker90;
                }
                else
                {
                    return leftLaneMarker90;
                }
            }else if(angle == 270)
            {
                if (direction == Direction.left)
                {
                    return leftLaneMarker90;
                }
                else
                {
                    return rightLaneMarker90;
                }
            }
            else
            {
                if (direction == Direction.up)
                {
                    return leftLaneMarker90;
                }
                else
                {
                    return rightLaneMarker90;
                }
            }
        }
    }
}
