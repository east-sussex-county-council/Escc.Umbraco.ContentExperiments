using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.Umbraco.ContentExperiments
{
    /// <summary>
    /// The customisations to apply to a page that is part of a Google Analytics content experiment
    /// </summary>
    public class ContentExperimentPageSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentExperimentPageSettings"/> class.
        /// </summary>
        public ContentExperimentPageSettings()
        {
            ScriptUrls = new List<Uri>();
            StylesheetUrls = new List<Uri>();
        }

        /// <summary>
        /// Gets or sets the content experiment script for the original page in the experiment, provided by Google Analytics
        /// </summary>
        /// <value>
        /// The content experiment script.
        /// </value>
        public IHtmlString ContentExperimentScript { get; set; }

        /// <summary>
        /// Gets the URLs of any custom scripts to run on the page under test.
        /// </summary>
        /// <value>
        /// The script urls.
        /// </value>
        public IList<Uri> ScriptUrls { get; private set; }

        /// <summary>
        /// Gets the URLs of any custom stylesheets to apply on the page under test.
        /// </summary>
        /// <value>
        /// The stylesheet urls.
        /// </value>
        public IList<Uri> StylesheetUrls { get; private set; } 
    }
}