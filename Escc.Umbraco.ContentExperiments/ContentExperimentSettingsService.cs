using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exceptionless.Extensions;
using Umbraco.Core;
using Umbraco.Web;

namespace Escc.Umbraco.ContentExperiments
{
    /// <summary>
    /// Query the Google Analytics content experiment settings
    /// </summary>
    public class ContentExperimentSettingsService : IContentExperimentSettingsService
    {
        /// <summary>
        /// Looks up the content experiment settings for page.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Scripts and styles, or <c>null</c> if this page is not being experimented upon</returns>
        public ContentExperimentPageSettings LookupSettingsForPage(int nodeId)
        {
            var relationToContentExperiment = ApplicationContext.Current.Services.RelationService.GetByChildId(nodeId).FirstOrDefault(r => r.RelationType.Alias == ContentExperimentPageRelationType.RelationTypeAlias);
            if (relationToContentExperiment == null) return null;

            var settingsPage = UmbracoContext.Current.ContentCache.GetById(relationToContentExperiment.ParentId);
            if (settingsPage == null) return null;

            var settings = new ContentExperimentPageSettings();

            settings.ContentExperimentScript = new HtmlString(settingsPage.GetPropertyValue<string>("contentExperiment_Content"));

            var urlParser = new ListOfUrlsFieldParser();
            urlParser.ParseUrls(settingsPage.GetPropertyValue<string>("stylesheetUrls"), settings.StylesheetUrls);
            urlParser.ParseUrls(settingsPage.GetPropertyValue<string>("javascriptUrls"), settings.ScriptUrls);
            
            return settings;
        }

        
    }
}