using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public abstract class Pirate : MonoBehaviour
    {

        /// <summary>
        /// current hp of pirate
        /// </summary>
        [SerializeField]
        protected float _helf;

        [SerializeField] protected PirateMover _mover;

        /// <summary>
        /// is this the captain?
        /// </summary>
        public abstract bool IsTheCaptain { get; }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void Awake()
        {
            _mover = GetComponent<PirateMover>();
        }
    }
}