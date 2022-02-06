using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Code
{
    public class TestLootController : MonoBehaviour
    {
        [SerializeField] private float _maxRadius = 4;
        [SerializeField] private float _minRadius = 2;
        [SerializeField] private Transform _start, _center, _end;
        [SerializeField] private int _count = 15;

        [SerializeField] private TestLootMovement _testTemplate;
        [SerializeField] private GameObject _sphereTemplate;

        private int lootCount = 6;

        private void Start()
        {
            for (int i = 0; i < lootCount; i++)
            {
                Vector3 endPosition = GetRandomPointOnRadius();
                Vector3 centerPosition = (transform.position + endPosition) * .5f;
                Vector3 centerPositionWithOffset = (transform.position + centerPosition) * .5f;

                endPosition += new Vector3(0, 0.1f);
                
                Instantiate(_sphereTemplate, centerPositionWithOffset, Quaternion.identity, transform);
                
                TestLootMovement loot = Instantiate(_testTemplate, transform.position, Quaternion.identity, transform);
                loot.Init(centerPosition, endPosition);
                loot.StartMoving();
            }
        }

        private Vector3 GetRandomPointOnRadius()
        {
            float radius = Random.Range(_minRadius, _maxRadius);
            
            float randomAngle = Random.Range(0f, 360f);
            float x = radius * Mathf.Cos(randomAngle);
            float z = radius * Mathf.Sin(randomAngle);

            return new Vector3(transform.position.x + x, 0f, transform.position.z + z);
        }

        private void OnDrawGizmos() {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, _maxRadius);
            
            foreach (var point in EvaluateSlerpPoints(_start.position, _end.position, _center.position,_count)) {
                Gizmos.DrawSphere(point, 0.1f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_center.position, 0.2f);
        }

        IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, Vector3 center,int count = 10) {
            Vector3 startRelativeCenter = start - center;
            Vector3 endRelativeCenter = end - center;

            float f = 1f / count;

            for (var i = 0f; i < 1 + f; i += f) {
                yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + center;
            }
        }
    }
}
