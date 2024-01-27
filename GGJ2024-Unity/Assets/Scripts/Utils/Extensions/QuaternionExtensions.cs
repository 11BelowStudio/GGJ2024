#region

using UnityEngine;

#endregion

// ReSharper disable once CheckNamespace
namespace Scripts.Utils.Extensions
{

    namespace QuaternionExt
    {

        /// <summary>
        /// Extension methods for Quaternions
        /// </summary>
        public static class QuaternionExtensions
        {
            /// <summary>
            /// It converts a Quaternion to a Vector4
            /// </summary>
            /// <param name="quat">The quaternion to convert to a Vector4.</param>
            /// <returns>
            /// A Vector4 of the quaternion's {x,y,z,w}
            /// </returns>
            public static Vector4 ToVector4(this Quaternion quat)
            {
                return new Vector4(quat.x, quat.y, quat.z, quat.w);
            }

            /// <summary>
            /// It takes a Quaternion and returns an array of floats
            /// </summary>
            /// <param name="quat">The quaternion to convert to an array.</param>
            /// <returns>
            /// An array of {x,y,z,w} from the quaternion
            /// </returns>
            public static float[] ToArray(this Quaternion quat)
            {
                return new[] { quat.x, quat.y, quat.z, quat.w };
            }

            /// <summary>
            /// It takes a quaternion and an array of floats, and it puts the quaternion's x, y, z, and w values into the
            /// array, in the order {x,y,z,w}, starting at the <see cref="offset"/> index, ending at index `offset+3`
            /// </summary>
            /// <param name="quat">The quaternion to put into the array.</param>
            /// <param name="arr">The array to copy the values to.</param>
            /// <param name="offset">The index of the array to start at</param>
            public static void ToArrayNonAlloc(this Quaternion quat, ref float[] arr, int offset = 0)
            {
                arr[offset] = quat.x;
                arr[offset + 1] = quat.y;
                arr[offset + 2] = quat.z;
                arr[offset + 3] = quat.w;
            }
        }

    }
}