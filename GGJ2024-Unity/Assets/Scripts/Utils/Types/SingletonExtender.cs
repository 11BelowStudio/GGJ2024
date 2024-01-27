#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Scripts.Utils.Types
{
    /// <summary>
    /// MonoBehaviour that can be attached to another GameObject with a different component on it to effectively
    /// make that component a singleton in a very janky way
    /// </summary>
    public class SingletonExtender: MonoBehaviour
    {
        /// <summary>
        /// The component that is being made into a singleton.
        /// </summary>
        [SerializeField]
        private Component makeThisASingleton;

        /// <summary>
        /// All of the fake singletons that the SingletonExtender class has made singletons
        /// </summary>
        private static IDictionary<Type, Component> _singletons = new Dictionary<Type, Component>();
        
        /// <summary>
        /// If the GameObject that the SingletonExtender is attached to is not the same as the GameObject that the thing
        /// you're trying to make into a singleton is attached to, then throw an error
        /// </summary>
        private void OnValidate()
        {
            if (makeThisASingleton.gameObject != gameObject)
            {
                Debug.LogError("Please attach the SingletonExtender to the same GameObject as the thing you're trying to make into a singleton!");
                makeThisASingleton = null;
            }
        }

        /// <summary>
        /// If the object is not a singleton, destroy it
        /// </summary>
        /// <returns>
        /// The value of the key in the dictionary.
        /// </returns>
        private void Awake()
        {
            
            if (makeThisASingleton == null){ Destroy(this); return; }
            
            if (_singletons.TryGetValue(makeThisASingleton.GetType(), out var existing))
            {
                if (existing != makeThisASingleton){ Destroy(gameObject); }
                return;
            }

            _singletons[makeThisASingleton.GetType()] = makeThisASingleton;

            foreach (var kv in _singletons)
            {
                Debug.Log(kv);
            }
            
        }

        /// <summary>
        /// If the object that is being destroyed is the same as the object that is stored in the dictionary, then remove
        /// the object from the dictionary
        /// </summary>
        private void OnDestroy()
        {
            if (_singletons.TryGetValue(makeThisASingleton.GetType(), out var existing))
            {
                if (existing == makeThisASingleton) { _singletons[makeThisASingleton.GetType()] = null; }
            }
            
        }
    }
}