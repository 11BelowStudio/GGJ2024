﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// The bits of the pirate which take damage when they get hit by a feathery sword
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PirateHurtbox : MonoBehaviour
    {
        /// <summary>
        /// Who does this hurtbox belong to?
        /// </summary>
        [SerializeField] private Pirate _myPirate;

        [SerializeField] private Collider _myCollider;

        private void OnValidate()
        {
            _myPirate = GetComponentInParent<Pirate>();
            _myCollider = GetComponent<Collider>();
        }

        private void Awake()
        {
            _myPirate = GetComponentInParent<Pirate>();
            _myCollider = GetComponent<Collider>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<FeatherSwordHitbox>(out FeatherSwordHitbox theSword))
            {

                if (theSword.MyPirate.IsDed)
                {
                    // swords do not work if the holder is ded. scientific fact!
                    return;
                }
                
                if (theSword.IsPlayerSword)
                {
                    if (_myPirate.IsTheCaptain)
                    {
                        // the captain cannot be harmed with their own sword (arr)
                        return;
                    }

                }
                else if (theSword.MyPirate == _myPirate)
                {
                    // no pirate may harm 'emselves wit' their own sword (arr)
                    return;
                }

                Vector3 impulse = collision.impulse;

                float relativeVelMag = collision.relativeVelocity.magnitude;
                float impulseMag = impulse.magnitude;

                Debug.Log($"Avast! Ye scallywag {_myPirate.name} 'ave been 'it wit' a relative velocity of {relativeVelMag} and an impulse of {impulseMag}, arr");

                // TODO: idk work out if the impulse is applying more to the sword or to the hurtbox
                // TODO: if it's applying more to hurtbox -> bigger hit (more damage)
                // TODO: if it's applying more to sword -> lesser hit (less damage)


                // deals the impulse force as damage
                _myPirate.HurtMe(impulseMag);

                /*
                Vector3 contactPos = collision.GetContact(0).point;

                Vector3 contactToHurtboxNorm = (transform.position - contactPos).normalized;
                Vector3 contactToSwordNorm = (theSword.transform.position - contactPos).normalized;
                */

            }
            else
            {
                // don't care if it was something else
                return;
            }
        }
    }
}