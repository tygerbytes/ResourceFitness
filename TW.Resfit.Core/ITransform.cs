// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransform.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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

        bool WillAffect(string fileText, Resource originalResource);
    }
}
