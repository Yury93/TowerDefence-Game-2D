using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDeffense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private AIPointPatrol[] points;

        public int Length { get { return points.Length; } }

        public AIPointPatrol this[int i] { get => points[i]; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i].transform.position, points[i].Radius);
            }
        }
    }
}