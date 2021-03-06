﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorFactory.cs" company="ZinkoSoft">
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
//   Factory for creating our editor object. Extends from the IVsEditoryFactory interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZinkoSoft.NuSet
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    /// <summary>
    /// Factory for creating our editor object. Extends from the IVsEditorFactory interface
    /// </summary>
    [Guid(GuidList.NuSetEditorFactoryGuidString)]
    public sealed class EditorFactory : IVsEditorFactory, IDisposable
    {
        /// <summary>
        /// The editor package.
        /// </summary>
        private readonly NuSetPackage editorPackage;

        /// <summary>
        /// The Visual Studio service provider.
        /// </summary>
        private ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorFactory" /> class.
        /// </summary>
        /// <param name="package">The package.</param>
        public EditorFactory(NuSetPackage package)
        {
            Debug.WriteLine(CultureInfo.CurrentCulture.ToString(), "Entering {0} constructor", this);

            this.editorPackage = package;
        }

        /// <summary>
        /// Since we create a ServiceProvider which implements IDisposable we
        /// also need to implement IDisposable to make sure that the ServiceProvider's
        /// Dispose method gets called.
        /// </summary>
        public void Dispose()
        {
            if (this.serviceProvider != null)
            {
                this.serviceProvider.Dispose();
            }
        }

        /// <summary>
        /// The set site.
        /// </summary>
        /// <param name="psp">The service provider.</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            this.serviceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// The get service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        // This method is called by the Environment (inside IVsUIShellOpenDocument::
        // OpenStandardEditor and OpenSpecificEditor) to map a LOGICAL view to a 
        // PHYSICAL view. A LOGICAL view identifies the purpose of the view that is
        // desired (e.g. a view appropriate for Debugging [LOGVIEWID_Debugging], or a 
        // view appropriate for text view manipulation as by navigating to a find
        // result [LOGVIEWID_TextView]). A PHYSICAL view identifies an actual type 
        // of view implementation that an IVsEditorFactory can create. 
        //
        // NOTE: Physical views are identified by a string of your choice with the 
        // one constraint that the default/primary physical view for an editor  
        // *MUST* use a NULL string as its physical view name (*physicalView = NULL).
        //
        // NOTE: It is essential that the implementation of MapLogicalView properly
        // validates that the LogicalView desired is actually supported by the editor.
        // If an unsupported LogicalView is requested then E_NOTIMPL must be returned.
        //
        // NOTE: The special Logical Views supported by an Editor Factory must also 
        // be registered in the local registry hive. LOGVIEWID_Primary is implicitly 
        // supported by all editor types and does not need to be registered.
        // For example, an editor that supports a ViewCode/ViewDesigner scenario
        // might register something like the following:
        //        HKLM\Software\Microsoft\VisualStudio\<version>\Editors\
        //            {...guidEditor...}\
        //                LogicalViews\
        //                    {...LOGVIEWID_TextView...} = s ''
        //                    {...LOGVIEWID_Code...} = s ''
        //                    {...LOGVIEWID_Debugging...} = s ''
        //                    {...LOGVIEWID_Designer...} = s 'Form'

        /// <summary>
        /// The map logical view.
        /// </summary>
        /// <param name="referencedGuidLogicalView">
        /// The referenced GUID logical view.
        /// </param>
        /// <param name="physicalView">
        /// The physical view.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int MapLogicalView(ref Guid referencedGuidLogicalView, out string physicalView)
        {
            // initialize out parameter
            physicalView = null;    

            // primary view uses NULL as physicalView
            if (VSConstants.LOGVIEWID_Primary == referencedGuidLogicalView)
            {
                return VSConstants.S_OK;        
            }
            
            return VSConstants.LOGVIEWID.TextView_guid == referencedGuidLogicalView ? VSConstants.S_OK : VSConstants.E_NOTIMPL;
        }

        /// <summary>
        /// The close.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public int Close()
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Used by the editor factory to create an editor instance. the environment first determines the
        /// editor factory with the highest priority for opening the file and then calls
        /// IVsEditorFactory.CreateEditorInstance. If the environment is unable to instantiate the document data
        /// in that editor, it will find the editor with the next highest priority and attempt to so that same
        /// thing.
        /// NOTE: The priority of our editor is 32 as mentioned in the attributes on the package class.
        /// Since our editor supports opening only a single view for an instance of the document data, if we
        /// are requested to open document data that is already instantiated in another editor, or even our
        /// editor, we return a value VS_E_INCOMPATIBLEDOCDATA.
        /// </summary>
        /// <param name="grfCreateDoc">Flags determining when to create the editor. Only open and silent flags
        /// are valid</param>
        /// <param name="pszMkDocument">path to the file to be opened</param>
        /// <param name="pszPhysicalView">name of the physical view</param>
        /// <param name="visualStudioHierarchy">pointer to the IVsHierarchy interface</param>
        /// <param name="itemid">Item identifier of this editor instance</param>
        /// <param name="punkDocDataExisting">This parameter is used to determine if a document buffer
        /// (DocData object) has already been created</param>
        /// <param name="ppunkDocView">Pointer to the IUnknown interface for the DocView object</param>
        /// <param name="ppunkDocData">Pointer to the IUnknown interface for the DocData object</param>
        /// <param name="pbstrEditorCaption">Caption mentioned by the editor for the doc window</param>
        /// <param name="commandUserInterfaceGuid">the Command UI GUID. Any UI element that is visible in the editor has
        /// to use this GUID. This is specified in the .VSCT file</param>
        /// <param name="flagsForCreateDocumentsWindow">Flags for CreateDocumentWindow</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public int CreateEditorInstance(
                        uint grfCreateDoc,
                        string pszMkDocument,
                        string pszPhysicalView,
                        IVsHierarchy visualStudioHierarchy,
                        uint itemid,
                        IntPtr punkDocDataExisting,
                        out IntPtr ppunkDocView,
                        out IntPtr ppunkDocData,
                        out string pbstrEditorCaption,
                        out Guid commandUserInterfaceGuid,
                        out int flagsForCreateDocumentsWindow)
        {
            Debug.WriteLine(CultureInfo.CurrentCulture.ToString(), "Entering {0} CreateEditorInstace()", this);

            // Initialize to null
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            commandUserInterfaceGuid = GuidList.NuSetEditoryFactoryGuid;
            flagsForCreateDocumentsWindow = 0;
            pbstrEditorCaption = null;

            // Validate inputs
            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            // Create the Document (editor)
            var newEditor = new EditorPane(this.editorPackage);
            ppunkDocView = Marshal.GetIUnknownForObject(newEditor);
            ppunkDocData = Marshal.GetIUnknownForObject(newEditor);
            pbstrEditorCaption = string.Empty;
            return VSConstants.S_OK;
        }
    }
}