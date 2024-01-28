using Scripts.Utils.Placeholders;
using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// hitbox for a feather sword
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class FeatherSwordHitbox : MonoBehaviour
    {
        /// <summary>
        /// true if this sword belongs to the player
        /// </summary>
        [SerializeField]
        private bool _isPlayerSword;

        /// <summary>
        /// true if this is the player's sword
        /// </summary>
        public bool IsPlayerSword => _isPlayerSword;

        /// <summary>
        /// reference to owner pirate
        /// </summary>
        [SerializeField]
        private Pirate _myPirate;

        /// <summary>
        /// returns owner pirate
        /// </summary>
        public Pirate MyPirate => _myPirate;

        [SerializeField] private AudioHolder _swordHitAudioHolder;

        [SerializeField] private AudioSource _swordHitAudioSource;

        private void OnValidate()
        {
            _myPirate = GetComponentInParent<Pirate>();
        }

        private void Awake()
        {
            _myPirate = GetComponentInParent<Pirate>();
        }

        private void PirateIsDed()
        {
            _myPirate.OnDed -= PirateIsDed;
            this.enabled = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        public void PlaySwordHitAudio()
        {
            if (_swordHitAudioHolder.TryGetRandomAudioClip(out AudioClip hitNoise))
            {
                _swordHitAudioSource.PlayOneShot(hitNoise);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}