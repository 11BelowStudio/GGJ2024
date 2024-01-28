using System.Collections;
using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// Heavily borrows from https://github.com/Firnox/Billboarding/blob/main/Billboard.cs
    /// </summary>
    public class TotesLegitPirateHat : MonoBehaviour
    {

        [SerializeField]
        private SpriteRenderer _sRend;


        [SerializeField] private BillboardType billboardType;

        [Header("Lock Rotation")]
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY;
        [SerializeField] private bool lockZ;

        private Vector3 originalRotation;

        public enum BillboardType { LookAtCamera, CameraForward };

        private void Awake()
        {
            originalRotation = transform.rotation.eulerAngles;
        }

        internal void SetVisible(bool newVisible)
        {
            _sRend.enabled = newVisible;
        }


        // Use Late update so everything should have finished moving.
        void LateUpdate()
        {
            // There are two ways people billboard things.
            switch (billboardType)
            {
                case BillboardType.LookAtCamera:
                    transform.LookAt(Camera.main.transform.position, Vector3.up);
                    transform.Rotate(0, 180, 0, Space.Self);
                    break;
                case BillboardType.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                default:
                    break;
            }
            // Modify the rotation in Euler space to lock certain dimensions.
            Vector3 rotation = transform.rotation.eulerAngles;
            if (lockX) { rotation.x = originalRotation.x; }
            if (lockY) { rotation.y = originalRotation.y; }
            if (lockZ) { rotation.z = originalRotation.z; }
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}