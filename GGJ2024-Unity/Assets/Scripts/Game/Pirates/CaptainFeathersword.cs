using System;
using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// The captain is here!
    /// 
    /// also the captain is a singleton.
    /// </summary>  
    public class CaptainFeathersword : Pirate
    {

        /// <summary>
        /// the captain is the captain
        /// </summary>
        public override bool IsTheCaptain { get { return true; } }

        private static CaptainFeathersword _instance;

        /// <summary>
        /// attempts to obtain the instance (may be null!)
        /// </summary>
        public static CaptainFeathersword Instance { get { return _instance; } }


        private const float _healthRegenDelayLength = 5f;
        private const float _healthRegenRate = 2.5f;
        private float _timeUntilHealthRegen = 0f;

        [SerializeField] private AudioSource _gameCameraAudioSource;
        public AudioSource GameCameraAudioSource => _gameCameraAudioSource;

        /// <summary>
        /// try get version of the instance obtainer
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool TryGetInstance(out CaptainFeathersword instance)
        {
            instance = _instance;
            return (null != instance);
        }


        private void OnTakenNonLethalDamage()
        {
            // if we take non-lethal damage, reset time until we can regen health
            _timeUntilHealthRegen = _healthRegenDelayLength;
        }

        

        protected override void Awake()
        {
            base.Awake();
            if (_instance == null)
            {
                _instance = this;
            } else
            {
                Destroy(this);
            }

            base.OnTakenDamage += OnTakenNonLethalDamage;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_instance == this)
            {
                _instance = null;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (base.IsDed)
            {
                return;
            }

            // health regen stuff
            if (_timeUntilHealthRegen > 0f)
            {
                _timeUntilHealthRegen -= Time.deltaTime;
            }
            else if (_helf < _maxHelf)
            {
                _helf = Mathf.Clamp(_helf + (_healthRegenRate * Time.deltaTime), 0, _maxHelf);

                OnHealthChanged01?.Invoke(_helf / _maxHelf);
            }

            // and input stuff

            float vertical = 0;
            if (Input.GetKey(KeyCode.W)) vertical += 1;
            if (Input.GetKey(KeyCode.S)) vertical -= 1;

            float horizontal = 0;
            if (Input.GetKey(KeyCode.D)) horizontal += 1;
            if (Input.GetKey(KeyCode.A)) horizontal -= 1;

            _mover.verticalInput = vertical;
            _mover.horizontalInput = horizontal;

            if (UnityEngine.Input.GetMouseButtonDown(1) || UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                _mover.DoAttack1();
                DoAttackAudio();
            }
            if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                _mover.DoAttack2();
                DoAttackAudio();
            }
            if (UnityEngine.Input.GetMouseButtonDown(2) || UnityEngine.Input.GetKeyDown(KeyCode.T) || UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                _mover.DoTPose();
                DoAttackAudio();
            }
            /*
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                _mover.DoDed();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                _mover.DoNotDed();
            }*/
            
        }
    }
}