// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceReplacementTransform.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceReplacementTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    public class ResourceReplacementTransform : ITransform
    {
        private readonly Resource replacementResource;

        public ResourceReplacementTransform(Resource replacementResource)
        {
            this.replacementResource = replacementResource;
        }

        public Resource Replacement
        {
            get
            {
                return this.replacementResource;
            }
        }

        public void Transform(Resource originalResource)
        {
            throw new System.NotImplementedException();
        }
    }
}