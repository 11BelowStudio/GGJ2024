using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// The bits of the pirate which take damage when they get hit by a feathery sword
    /// </summary>
    public class PirateHurtbox : MonoBehaviour
    {
        /// <summary>
        /// Who does this hurtbox belong to?
        /// </summary>
        [SerializeField] private Pirate _myPirate;



        private void OnValidate()
        {
            _myPirate = GetComponentInParent<Pirate>();
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

                float relativeVelMag = collision.relativeVelocity.magnitude;
                float impulseMag = collision.impulse.magnitude;

                Debug.Log($"Avast! Ye scallywag {_myPirate.name} 'ave been 'it wit' a relative velocity of {relativeVelMag} and an impulse of {impulseMag}, arr");
            }
            else
            {
                // don't care if it was something else
                return;
            }
        }
    }
}