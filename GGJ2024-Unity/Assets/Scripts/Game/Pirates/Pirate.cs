using System;
using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// generic class for piratey things
    /// </summary>
    public abstract class Pirate : MonoBehaviour
    {

        /// <summary>
        /// current hp of pirate
        /// </summary>
        [SerializeField]
        protected float _helf;

        [SerializeField] protected float _maxHelf;

        /// <summary>
        /// length of brief invincibility after taking damage
        /// </summary>
        [SerializeField] protected float _gracePeriodLength = 0.5f;

        /// <summary>
        /// how much of the grace period is left
        /// </summary>
        private float _gracePeriodLeft = -1f;

        [SerializeField] protected PirateMover _mover;

        /// <summary>
        /// is this the captain?
        /// </summary>
        public abstract bool IsTheCaptain { get; }

        /// <summary>
        /// is this pirate ded (from laughter)
        /// </summary>
        [SerializeField] protected bool _isDed = false;

        /// <summary>
        /// is this pirate ded (from laughter)
        /// </summary>
        public bool IsDed => _isDed;


        /// <summary>
        /// Invoked when this pirate dies
        /// </summary>
        public Action OnDed;

        /// <summary>
        /// invoked upon taking damage but not ded
        /// </summary>
        public Action OnTakenDamage;

        /// <summary>
        /// invoked when pirate health changes (returns current health as proportion of max health)
        /// </summary>
        public Action<float> OnHealthChanged01;

        // Use this for initialization
        protected virtual void Start()
        {

        }


        // Update is called once per frame
        protected virtual void Update()
        {
            if (_gracePeriodLeft >= 0f)
            {
                _gracePeriodLeft -= Time.deltaTime;
            }
        }

        protected virtual void Awake()
        {
            _isDed = false;
            _mover = GetComponent<PirateMover>();
        }

        protected virtual void OnDestroy()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnValidate()
        {

        }

        protected virtual void OnDisable()
        {

        }


        public virtual void HurtMe(float damageToDeal, bool respectGracePeriod = true)
        {
            if (respectGracePeriod && _gracePeriodLeft  > 0f)
            {
                // arr, no 'arrmin' another pirate during thar grace period, arr...
                return;
            }

            _helf = Mathf.Clamp(_helf - damageToDeal, 0f, _maxHelf);


            OnHealthChanged01?.Invoke(Mathf.Clamp01(_helf / _maxHelf));

            if (_helf <= 0)
            {
                _mover.DoDed(); // you are ded
                OnDed?.Invoke();
                _isDed = true;
            }
            else
            {
                OnTakenDamage?.Invoke();
                if (respectGracePeriod)
                {
                    _gracePeriodLeft = _gracePeriodLength;
                }
            }
            
        }
    }
}