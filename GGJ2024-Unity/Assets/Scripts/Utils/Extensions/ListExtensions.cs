#region

using System;
using System.Collections.Generic;

#endregion

// ReSharper disable once CheckNamespace
namespace Scripts.Utils.Extensions
{

    namespace ListExt
    {
        /// <summary>
        /// Extensions for List/IList
        /// </summary>
        public static class ListExtensions
        {
            /// <summary>
            /// If the list contains the item, do nothing. Otherwise, add the item to the list
            /// </summary>
            /// <param name="theList">The list to add to</param>
            /// <param name="addMe">The item that might be added to the list</param>
            public static void AddIfNotPresent<T>(this IList<T> theList, T addMe)
            {
                if (theList.Contains(addMe)) return;
                theList.Add(addMe);
            }

            /// <summary>
            /// It takes a list and an enumerable of items to add to the list, and returns the list with the items added,
            /// but only if they are not already present in the list
            /// </summary>
            /// <param name="theList">The list you want to add to.</param>
            /// <param name="addThese">The items to add to the list.</param>
            /// <returns>
            /// The list itself.
            /// </returns>
            public static IList<T> AddAllIfNotPresent<T>(this List<T> theList, IEnumerable<T> addThese)
            {
                var uniques = new HashSet<T>(theList);
                uniques.UnionWith(addThese);
                theList.Clear();
                theList.InsertRange(0, uniques);
                return theList;
            }

            /// <summary>
            /// Swaps the items at index A and index B of the list. Also returns the item that was at index A.
            /// </summary>
            /// <param name="theList"></param>
            /// <param name="a">index of the first iem (item that was at this index will be returned)</param>
            /// <param name="b">index to swap that item with.</param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T SwapTheseTwoAndGet<T>(this IList<T> theList, int a, int b = 0)
            {
                var getMe = theList[a];
                theList[a] = theList[b];
                theList[b] = getMe;
                return getMe;
            }



            /// <summary>
            /// If the list contains an element that matches the condition, return the first match. Otherwise, return the
            /// first element
            /// </summary>
            /// <param name="theList">The list you want to search through.</param>
            /// <param name="condition">A predicate that returns true if the item matches the condition.</param>
            /// <returns>
            /// The first element in the list that matches the condition (or the item at index 0).
            /// </returns>
            public static T GetFirstMatchOrFirstElement<T>(this IList<T> theList, Predicate<T> condition)
            {
                foreach (var item in theList)
                {
                    if (condition(item))
                    {
                        return item;
                    }
                }

                return theList[0];

            }


        }
    }
}