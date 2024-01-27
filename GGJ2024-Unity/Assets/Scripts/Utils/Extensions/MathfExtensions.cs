
// ReSharper disable once CheckNamespace
namespace Scripts.Utils.Extensions
{

    namespace Mathf
    {
        /// <summary>
        /// Some more math-related functions
        /// </summary>
        public static class MathfExtensions
        {

            /// <summary>
            /// ReLU activation function. Returns max(f, 0)
            /// </summary>
            /// <param name="f">float to apply ReLU to</param>
            /// <returns>max(0, f)</returns>
            // ReSharper disable once InconsistentNaming
            public static float ReLU(in float f)
            {
                return UnityEngine.Mathf.Max(f, 0.0f);
            }

            /// <summary>
            /// If the denominator is 0, return 0, otherwise return the result of the division
            /// </summary>
            /// <param name="numerator">The number to be divided.</param>
            /// <param name="denominator">The number to divide by.</param>
            /// <returns>
            /// The return value is the result of the division of the numerator by the denominator.
            /// </returns>
            public static float SafeDiv(this float numerator, float denominator)
            {
                return (denominator.CompareTo(0d) == 0) ? 0 : numerator / denominator;
            }

            /// <summary>
            /// ReLU activation function. Returns max(i, 0)
            /// </summary>
            /// <param name="f">float to apply ReLU to</param>
            /// <returns>max(f, 0)</returns>
            // ReSharper disable once InconsistentNaming
            public static float ReLU(this float f)
            {
                return UnityEngine.Mathf.Max(f, 0.0f);
            }

            /// <summary>
            /// ReLU activation function. Returns max(i, 0)
            /// </summary>
            /// <param name="i">int to apply ReLU to</param>
            /// <returns>max(i, 0)</returns>
            // ReSharper disable once InconsistentNaming
            public static int ReLU(this int i)
            {
                return UnityEngine.Mathf.Max(i, 0);
            }

            /// <summary>
            /// ReLU activation function. Returns max(d, 0)
            /// </summary>
            /// <param name="d">double to apply ReLU to</param>
            /// <returns>max(d, 0)</returns>
            // ReSharper disable once InconsistentNaming
            public static double ReLU(in double d)
            {
                return d > 0d ? d : 0d;
            }

            public static double ReLU(this double d)
            {
                return d > 0d ? d : 0d;
            }
        }

    }
}