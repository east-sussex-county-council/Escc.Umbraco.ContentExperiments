using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.Umbraco.ContentExperiments.DocumentTypes
{
    /// <summary>
    /// Content tab for the 'Content experiment page' document type in Umbraco
    /// </summary>
    public class ContentExperimentPageContentTab : TabBase
    {
        [UmbracoProperty("Apply to page", "applyToPage", "Umbraco.ContentPickerAlias", "Content Picker", description: "Select the target page on which to apply these settings", sortOrder: 1)]
        public string ApplyToPage { get; set; }

        [UmbracoProperty("Google Analytics Content Experiment script", "contentExperiment", BuiltInUmbracoDataTypes.TextboxMultiple, description: "If this is targeting the original page in a Google Analytics Content Experiment, paste the code here.", sortOrder: 2)]
        public string ContentExperiment { get; set; }
        
        [UmbracoProperty("Stylesheets to load", "stylesheetUrls", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any stylesheet URLs, one per line. Each URL must start with /css/. Stylesheets can be created in the Settings section.", sortOrder: 3)]
        public string WhereElseToDisplayIt { get; set; }

        [UmbracoProperty("Scripts to load", "javascriptUrls", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any JavaScript URLs, one per line.  Each URL must start with /scripts/. Scripts can be created in the Settings section.", sortOrder: 4)]
        public string JavaScriptUrls { get; set; }
    }
}
