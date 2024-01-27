using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    /// <summary>
    /// Give this any sort of list, and it will shuffle it.
    /// </summary>
    public static class ListShuffler
    {   
        
        /// <summary>
        /// Shuffle a list using UnityEngine.Random
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleUnityRNG<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n+1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }



        /// <summary>
        /// C# random instance
        /// </summary>
        private static System.Random rng = new System.Random();

        

        /// <summary>
        /// Shuffle a list using C# Random
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleCsRNG<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


    }
}