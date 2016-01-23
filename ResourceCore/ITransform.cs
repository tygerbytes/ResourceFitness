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
    public interface ITransform
    {
        void Transform(Resource originalResource);
    }
}