using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.Umbraco.ContentExperiments.DocumentTypes;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Exceptionless;
using Umbraco.Inception.CodeFirst;
using Umbraco.Web.WebApi;

namespace Escc.Umbraco.ContentExperiments.ApiControllers
{
    /// <summary>
    /// Create Umbraco document types needed for this project
    /// </summary>
    public class ContentExperimentsController : UmbracoApiController
    {
        /// <summary>
        /// Checks the authorisation token passed with the request is valid, so that this method cannot be called without knowing the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static bool CheckAuthorisationToken(string token)
        {
            return token == ConfigurationManager.AppSettings["Escc.Umbraco.Inception.AuthToken"];
        } 
        
        /// <summary>
        /// Creates the supporting types (eg data types) needed for <see cref="CreateUmbracoDocumentTypes"/> to succeed.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoSupportingTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                // Insert data types before the document types that use them, otherwise the relevant property is not created
                RichTextAuthorNotesDataType.CreateDataType();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Creates the Umbraco document types defined by this project.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoDocumentTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(ContentExperimentPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(ContentExperimentsDocumentType));
 
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
