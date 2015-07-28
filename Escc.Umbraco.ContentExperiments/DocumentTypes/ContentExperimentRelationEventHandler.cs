using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Escc.Umbraco.ContentExperiments.DocumentTypes
{
    /// <summary>
    /// Maintain an Umbraco relation between a content experiment settings page, and the content node it should be applied to
    /// </summary>
    public class ContentExperimentRelationEventHandler : ApplicationEventHandler
    {
        const string RelationTypeAlias = "Escc.Umbraco.ContentExperiments.Page";
        private const string TargetPageIdPropertyAlias = "applyToPage_Content";

        /// <summary>
        /// Overridable method to execute when Bootup is completed, this allows you to perform any other bootup logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                EnsureRelationTypeExists();

                ContentService.Saved += ContentService_Saved;
                ContentService.Copying += ContentService_Copying;
                ContentService.Trashed += ContentService_Trashed;
                ContentService.Deleting += ContentService_Deleting;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When copying a content experiment settings page, remove the target page, as only one content experiment settings page should relate to a given target page
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Umbraco.Core.Events.CopyEventArgs{IContent}"/> instance containing the event data.</param>
        void ContentService_Copying(IContentService sender, global::Umbraco.Core.Events.CopyEventArgs<IContent> e)
        {
            try
            {
                if (e.Copy.ContentType.Alias != "ContentExperimentPage") return;

                e.Copy.Properties[TargetPageIdPropertyAlias].Value = null;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When a content experiment settings page is saved, relate it to the page the settings are being applied to
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Umbraco.Core.Events.SaveEventArgs{IContent}"/> instance containing the event data.</param>
        void ContentService_Saved(IContentService sender, global::Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            try
            {
                foreach (var node in e.SavedEntities)
                {
                    if (node.ContentType.Alias != "ContentExperimentPage") continue;

                    RemoveExistingRelation(node);

                    AddNewRelation(node);
                }

            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When a page is moved to the recycle bin, move its content experiment settings there too
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MoveEventArgs{IContent}"/> instance containing the event data.</param>
        void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            try
            {
                foreach (var moveInfo in e.MoveInfoCollection)
                {
                    var relations = ApplicationContext.Current.Services.RelationService.GetByChildId(moveInfo.Entity.Id).Where(r => r.RelationType.Alias == RelationTypeAlias);
                    foreach (var relation in relations)
                    {
                        var contentExperimentPage = ApplicationContext.Current.Services.ContentService.GetById(relation.ParentId);
                        ApplicationContext.Current.Services.ContentService.MoveToRecycleBin(contentExperimentPage);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Deletes the relation, and for a content node its content experiment settings page, when a page is deleted from the recycle bin
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DeleteEventArgs{IContent}"/> instance containing the event data.</param>
        private void ContentService_Deleting(IContentService sender, DeleteEventArgs<IContent> e)
        {
            try
            {
                foreach (var node in e.DeletedEntities)
                {
                    // If this is a content page linked to content experiment settings, delete the settings too
                    var relationsToContentExperiments = ApplicationContext.Current.Services.RelationService.GetByChildId(node.Id).Where(r => r.RelationType.Alias == RelationTypeAlias);
                    foreach (var relation in relationsToContentExperiments)
                    {
                        var contentExperimentPage = ApplicationContext.Current.Services.ContentService.GetById(relation.ParentId);
                        ApplicationContext.Current.Services.ContentService.Delete(contentExperimentPage);
                        ApplicationContext.Current.Services.RelationService.Delete(relation);
                    }

                    // If this is an experiment page, delete the relation but not the target page
                    var relationsFromContentExperiments = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == RelationTypeAlias);
                    foreach (var relation in relationsFromContentExperiments)
                    {
                        ApplicationContext.Current.Services.RelationService.Delete(relation);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Adds the new relation between a content experiment settings page and its content node.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void AddNewRelation(IContent node)
        {
            var applyToPage = node.Properties[TargetPageIdPropertyAlias];
            if (applyToPage != null && applyToPage.Value != null && !String.IsNullOrEmpty(applyToPage.Value.ToString()))
            {
                try
                {
                    var targetPageId = Int32.Parse(applyToPage.Value.ToString(), CultureInfo.InvariantCulture);
                    ApplicationContext.Current.Services.RelationService.Save(new Relation(node.Id, targetPageId, ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(RelationTypeAlias)));
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            }
        }

        /// <summary>
        /// Removes the existing relation between a content experiment settings page and its content node
        /// </summary>
        /// <param name="node">The node.</param>
        private static void RemoveExistingRelation(IContent node)
        {
            var relations = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == RelationTypeAlias);
            foreach (var relation in relations)
            {
                ApplicationContext.Current.Services.RelationService.Delete(relation);
            }
        }

        /// <summary>
        /// Ensures the relation type exists between a 'content experiment page' node and its target content node
        /// </summary>
        private void EnsureRelationTypeExists()
        {
            if (ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(RelationTypeAlias) == null)
            {
                var relationType = new RelationType(new Guid("c66ba18e-eaf3-4cff-8a22-41b16d66a972"), new Guid("c66ba18e-eaf3-4cff-8a22-41b16d66a972"), RelationTypeAlias);
                ApplicationContext.Current.Services.RelationService.Save(relationType);
            }
        }
    }
}