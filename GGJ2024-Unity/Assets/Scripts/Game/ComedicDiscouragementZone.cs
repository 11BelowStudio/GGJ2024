using Scripts.Game.Pirates;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// discourages touching this thing by causing anything touching it to take damage.
    /// </summary>
    public class ComedicDiscouragementZone : MonoBehaviour
    {

        [SerializeField] private float _damagePerSecond = 5f;

        public float DamagePerSecond => _damagePerSecond;

    }
}