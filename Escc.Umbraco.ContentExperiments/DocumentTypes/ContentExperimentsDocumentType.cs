using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.Umbraco.ContentExperiments.DocumentTypes
{
    /// <summary>
    /// Specification for the 'Content experiments' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Google Analytics content experiments", "ContentExperiments", new Type[] { typeof(ContentExperimentPageDocumentType) }, false, allowAtRoot: true, enableListView: true, icon: BuiltInUmbracoContentTypeIcons.IconSpeedGauge, Description = "Manage custom styles and scripts for Google Analytics content experiments.")]
    public class ContentExperimentsDocumentType : UmbracoGeneratedBase
    {
        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}