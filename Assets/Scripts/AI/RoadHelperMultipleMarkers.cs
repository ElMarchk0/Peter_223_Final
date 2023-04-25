using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCity.AI
{
    public class RoadHelperMultipleMarkers : RoadHelper
    {
        [SerializeField]
        List<Marker>  incomingMarkers;

       [SerializeField]
        List<Marker>  outGoingMarkers;

        public override Marker GetPositionForCarToSpawn(Vector3 nextPathPosition)
        {
            return base.GetClosestMarkerTo(nextPathPosition, outGoingMarkers);
        }

        public override Marker GetPositionForCarToEnd(Vector3 previousPathPosition)
        {
            return base.GetClosestMarkerTo(previousPathPosition, incomingMarkers);
        }
    }
}
