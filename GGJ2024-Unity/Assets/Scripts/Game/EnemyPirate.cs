using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class EnemyPirate : Pirate
    {

        /// <summary>
        /// the enemy is not the captain
        /// </summary>
        public override bool IsTheCaptain { get { return false; } }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}