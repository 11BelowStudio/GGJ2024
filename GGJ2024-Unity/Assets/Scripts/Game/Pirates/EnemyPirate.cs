using UnityEngine;

namespace Scripts.Game.Pirates
{
    public class EnemyPirate : Pirate
    {

        /// <summary>
        /// the enemy is not the captain
        /// </summary>
        public override bool IsTheCaptain { get { return false; } }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}