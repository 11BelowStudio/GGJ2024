#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

// ReSharper disable once CheckNamespace
namespace Scripts.Utils.Extensions
{
    namespace EnumerableExt
    {
        /// <summary>
        /// Extensions for IEnumerable
        /// </summary>
        public static class EnumerableExtensions
        {
            /// <summary>
            /// If the IEnumerable contains an object of type `T`, assign it to `findMe`, and return true.
            /// </summary>
            /// <param name="theList">The IEnumerable you want to search through.</param>
            /// <param name="findMe">The component you want to find</param>
            /// /// <returns>
            /// True if a component of that type could be found, false otherwise
            /// </returns>
            public static bool ContainsObjectOfType<T>(this IEnumerable<T> theList, Type findMe)
            {
                foreach (var item in theList)
                {
                    if (item == null) continue;
                    
                    if (item is Type) return true;
                }
                return false;
            }

            /// <summary>
            /// This function will return the first component of type `TR`
            /// found in the IEnumerable of components of type `T`
            /// </summary>
            /// <param name="theComponentList">The IEnumerable of components to search through.</param>
            /// <param name="findMe">The component you're looking for</param>
            /// <returns>
            /// True if a component of that type could be found, false otherwise
            /// </returns>
            public static bool TryGetComponentOfTypeInList<T, TR>(this IEnumerable<T> theComponentList, out TR findMe)
                where T : Component
                where TR : Component
            {
                foreach (var item in theComponentList)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    if (item.TryGetComponent(out TR comp))
                    {
                        findMe = comp;
                        return true;
                    }
                }

                findMe = null;
                return false;
            }
            
            /// <summary>
            /// It takes an enumerable of components, and populates a list of components of a given type from the
            /// initial enumerable
            /// </summary>
            /// <param name="theComponentList">The list of components to search through.</param>
            /// <param name="found">The list that will be populated with the found components.</param>
            public static void GetComponentsOfTypeInList<T, TR>(this IEnumerable<T> theComponentList, out List<TR> found)
                where T : Component
                where TR : Component
            {
                found = new List<TR>();
                foreach (var item in theComponentList)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    if (item.TryGetComponent(out TR comp))
                    {
                        found.Add(comp);
                    }
                }
            }
            
            /// <summary>
            /// Attempts to obtain first element from enumerable (or a default value if the enumerable is empty)
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="theEnumerable"></param>
            /// <param name="defaultVal"></param>
            /// <returns>First element from enumerable if not empty. Otherwise returns default value.</returns>
            public static T GetFirstElementOrDefault<T>(this IEnumerable<T> theEnumerable, T defaultVal)
            {
                var returnThis = defaultVal;
                var enumerator = theEnumerable.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    returnThis = enumerator.Current;
                    enumerator.Dispose();
                }
                return returnThis;
            }


            /// <summary>
            /// Attempts to obtain first matching element from enumerable (or a default value if it could not be found)
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="theEnumerable"></param>
            /// <param name="defaultVal"></param>
            /// <returns>First element from enumerable if not empty. Otherwise returns default value.</returns>
            public static T FindFirstMatchOrDefault<T>(this IEnumerable<T> theEnumerable, T defaultVal, Predicate<T> matchPredicate)
            {
                foreach(T item in theEnumerable)
                {
                    if (matchPredicate(item))
                    {
                        return item;
                    }
                }
                return defaultVal;
            }

        }
        
    }
}