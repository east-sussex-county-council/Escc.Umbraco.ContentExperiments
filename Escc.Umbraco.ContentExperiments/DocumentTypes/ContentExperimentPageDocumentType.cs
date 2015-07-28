using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.Umbraco.ContentExperiments.DocumentTypes
{
    /// <summary>
    /// Specification for the 'Content experiment page' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Google Analytics content experiment page", "ContentExperimentPage", new Type[0], false, allowAtRoot: false, enableListView: false, icon: BuiltInUmbracoContentTypeIcons.IconWand, Description = "Apply customisations to a page in a Google Analytics content experiment.")]
    public class ContentExperimentPageDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public ContentExperimentPageContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}