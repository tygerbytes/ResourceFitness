// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransform.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ITransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Xml.Linq;

    public interface ITransform
    {
        void Transform(ref string sourceFile, Resource originalResource);

        void Transform(ref XElement resourcesXml, Resource originalResource);

        Resource Transform(Resource originalResource);
    }
}