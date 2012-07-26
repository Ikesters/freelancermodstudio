﻿using System.Windows.Media.Media3D;

namespace HelixEngine
{
    /// <summary>
    /// Helper methods for Transform3D.
    /// </summary>
    public static class Transform3DHelper
    {
        #region Public Methods

        /// <summary>
        /// Combines two transforms.
        /// </summary>
        /// <param name="t1">
        /// The first transform.
        /// </param>
        /// <param name="t2">
        /// The second transform.
        /// </param>
        /// <returns>
        /// The combined transform group.
        /// </returns>
        public static Transform3D CombineTransform(Transform3D t1, Transform3D t2)
        {
            var g = new Transform3DGroup();
            g.Children.Add(t1);
            g.Children.Add(t2);
            return g;
        }

        #endregion
    }
}