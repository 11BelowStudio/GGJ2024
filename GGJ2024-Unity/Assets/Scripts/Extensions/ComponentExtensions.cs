#region

using UnityEngine;

#endregion

namespace Scripts.Utils.Extensions
{

    namespace ComponentExt
    {
        /// <summary>
        /// Rachel's shitty component extension methods
        /// </summary>
        public static class ComponentExtensions
        {

            /// <summary>
            /// obtains the component in children from the gameobject with the specified tag.
            /// </summary>
            /// <param name="g"></param>
            /// <param name="tag"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns>specified component from a child that has the given tag</returns>
            public static T GetComponentInChildrenWithTag<T>(this Component c, string tag) where T : class
            {
                foreach (Transform t in c.transform)
                {
                    if (t.CompareTag(tag)) return t.GetComponent<T>();
                }

                return null;
            }

            /// <summary>
            /// If the game object has the component, return it. Otherwise, return the component in the children or parent
            /// </summary>
            /// <param name="g">The game object to search for the component in.</param>
            /// <param name="comp">The component being searched for (will hold the component if found).</param>
            /// <param name="includeInactives">If true, will search inactive objects.</param>
            /// <returns>
            /// A bool indicating whether or not the component of that type was found in the hierarchy
            /// </returns>
            public static bool TryGetComponentInHierarchy<T>(this Component c, out T comp,
                bool includeInactives = false) where T : class
            {
                if (c.TryGetComponent(out T comp1))
                {
                    comp = comp1;
                    return true;
                }

                comp = c.GetComponentInChildren<T>(includeInactives);
                if (comp != null)
                {
                    return true;
                }

                comp = c.GetComponentInParent<T>(includeInactives);
                return comp != null;
            }

        }

    }
}
