#region

using System;

#endregion

// ReSharper disable once CheckNamespace
namespace Scripts.Utils.Extensions
{
    namespace IComparable
    {
        /// <summary>
        /// Extensions for IComparable
        /// </summary>
        public static class ComparableExtensions
        {

            /// <summary>
            /// This function returns true if the value of the object is greater than the lower value and less than the
            /// upper value
            /// </summary>
            /// <param name="me">The object that you're calling the extension method on.</param>
            /// <param name="lower">Lower bound</param>
            /// <param name="upper">Upper bound</param>
            /// <returns>
            /// True if lower &lt; me &lt; upper.
            /// </returns>
            public static bool IsBetween<T>(this IComparable<T> me, T lower, T upper)
            {
                return (me.CompareTo(lower) > 0) && (me.CompareTo(upper) < 0);
            }

            /// <summary>
            /// This function returns true if the value of the object is greater than or equal to the lower value and
            /// less than or equal to the upper value
            /// </summary>
            /// <param name="me">The object that you're calling the extension method on.</param>
            /// <param name="lower">Lower bound</param>
            /// <param name="upper">Upper bound</param>
            /// <returns>
            /// True if lower &lt;= me &lt;= upper.
            /// </returns>
            public static bool IsBetweenOrEqual<T>(this IComparable<T> me, T lower, T upper)
            {
                return (me.CompareTo(lower) >= 0) && (me.CompareTo(upper) <= 0);
            }

        }
    }
}