using System.Collections;
using UnityEngine;


namespace Scripts.Game.Manager
{
    public class Spawnpoint : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);
        }

        public Vector3 GetRandomlyOffsetPosition()
        {
            
            return transform.position + (UnityEngine.Random.onUnitSphere * Time.deltaTime);

        }
    }
}