using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleCity.AI
{
    public class Marker : MonoBehaviour
    {
        public Vector3 Position { get => transform.position;}

        public List<Marker> adjacentMarkers;

        // Set in unity if a marker is open for a connection
        [SerializeField]
        private bool openForConnections;

        public bool OpenForConnections
        {
            get { return openForConnections; }
        }

        // populate a list of adjacent markers
        public List<Vector3> GetAdjacentPositions()
        {
            return new List<Vector3>(adjacentMarkers.Select(x => x.Position).ToList());
        }

        // Draw connections between markers
        private void OnDrawGizmos()
        {
            if(Selection.activeObject == gameObject)
            {
                Gizmos.color = Color.red;
                if (adjacentMarkers.Count > 0)
                {
                    foreach (var item in adjacentMarkers)
                    {
                        Gizmos.DrawLine(transform.position, item.Position);
                    }
                }
                Gizmos.color = Color.white;
            }
        }
    }

}
