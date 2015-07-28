namespace Escc.Umbraco.ContentExperiments
{
    public interface IContentExperimentSettingsService
    {
        /// <summary>
        /// Looks up the content experiment settings for page.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Scripts and styles, or <c>null</c> if this page is not being experimented upon</returns>
        ContentExperimentPageSettings LookupSettingsForPage(int nodeId);
    }
}