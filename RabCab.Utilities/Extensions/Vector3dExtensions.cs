﻿// -----------------------------------------------------------------------------------
//     <copyright file="Vector3dExtensions.cs" company="CraterSpace">
//     Copyright (c) 2019 CraterSpace - All Rights Reserved 
//     </copyright>
//     <author>Zach Ayers</author>
//     <date>03/08/2019</date>
//     Description:    
//     Notes:  
//     References:          
// -----------------------------------------------------------------------------------

using Autodesk.AutoCAD.Geometry;

namespace RabCab.Utilities.Extensions
{
    /// <summary>
    ///     Provides extension methods for the Vector3d type.
    /// </summary>
    public static class Vector3DExtensions
    {
        /// <summary>
        ///     Projects the vector on the WCS
        /// </summary>
        /// <param name="vec">The vector to project.</param>
        /// <returns>The projected vector.</returns>
        public static Vector3d Flatten(this Vector3d vec)
        {
            return new Vector3d(vec.X, vec.Y, 0.0);
        }
    }
}