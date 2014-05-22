// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidList.cs" company="ZinkoSoft">
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The GUID list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZinkoSoft.NuSet
{
    using System;

    /// <summary>
    /// The GUID list.
    /// </summary>
    internal static class GuidList
    {
        /// <summary>
        /// The unique identifier NuSet PKG string
        /// </summary>
        public const string NuSetPackageGuidString = "721c8330-d7d8-417e-a9be-73337d4c7c6c";
        
        /// <summary>
        /// The unique identifier nu set command set string
        /// </summary>
        public const string NuSetCommandSetGuidString = "917f1e96-9b58-4832-8d6c-efe2d35eb2b6";
        
        /// <summary>
        /// The unique identifier tool window persistence string
        /// </summary>
        public const string ToolWindowsPersistenceGuidString = "94f39e53-7ad2-4735-b76f-f379fed83487";
        
        /// <summary>
        /// The unique identifier nu set editor factory string
        /// </summary>
        public const string NuSetEditorFactoryGuidString = "b7d1fbd6-7b4d-4872-93e5-92c3a427ca33";

        /// <summary>
        /// The unique identifier nu set command set
        /// </summary>
        public static readonly Guid NuSetCommandGuid = new Guid(NuSetCommandSetGuidString);
        
        /// <summary>
        /// The unique identifier nu set editor factory
        /// </summary>
        public static readonly Guid NuSetEditoryFactoryGuid = new Guid(NuSetEditorFactoryGuidString);
    }
}