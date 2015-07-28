using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace Escc.Umbraco.ContentExperiments
{
    /// <summary>
    /// An umbraco relation between a content experiment settings page and its target page
    /// </summary>
    public class ContentExperimentPageRelationType : RelationType
    {
        public const string RelationTypeAlias = "Escc.Umbraco.ContentExperiments.Page";
        private const string ContentNodeGuid = "c66ba18e-eaf3-4cff-8a22-41b16d66a972";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentExperimentPageRelationType"/> class.
        /// </summary>
        public ContentExperimentPageRelationType() : base(new Guid(ContentNodeGuid), new Guid(ContentNodeGuid), RelationTypeAlias)
        {
        }
    }
}