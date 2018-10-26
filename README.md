# Escc.Umbraco.ContentExperiments

This project allows you to manage custom styles and scripts for Google Analytics content experiments in Umbraco. Each Umbraco page in a content experiment has a corresponding page using the 'Content experiment page' document type, and these are kept under a common 'Content experiments' document type.

Create the 'Content experiments' document type and set it to use a list view and allow 'Content experiment' as the only child type. Set up the 'Content experiment page' document type with four fields: 

- A content picker with the alias `applyToPage_Content` to select the target page on which to apply the settings.
- A multi-line textbox with the alias `contentExperiment_Content` for the content experiment code, which is only used on page 1 of an experiment.
- A multi-line textbox with the alias `javascriptUrls_Content`, which is for a list of script URLs which should be loaded to customise the page under test, one URL per line.
- A multi-line textbox with the alias `stylesheetUrls_Content`, which is for a list of stylesheets which should be loaded to customise the page under test, one URL per line.

By listing URLs rather than entering JavaScript and CSS directly into the 'Content experiment page' document type, it's possible to use the Settings > Stylesheets and Settings > Scripts sections which offer syntax highlighting.

On any view which might have a content experiment applied to it, use the `ContentExperimentSettingsService` to look up whether there are any content experiment settings to apply:

	var service = new ContentExperimentSettingsService();
	model.ContentExperimentPageSettings = service.LookupSettingsForPage(umbracoContentNode.Id);

This will return either a `ContentExperimentPageSettings` object with the Google Analytics script, list of CSS files and list of JavaScripts that you need to add to your view, or `null` if there's no content experiment.

This project is published as a NuGet package and referenced by the Umbraco installation at the root of the website.

## How to create a content experiment

1. Create the original page and another version (or versions) you want to test.
2. In the Umbraco settings section create any scripts and styles you need to customise each version of the page.
3. For each page in the experiment, create and publish an Umbraco page using the 'Content experiment page' document type. Add the CSS and JavaScript URLs for that page, and select the target page to apply them to. Publish your target pages, which should now have their custom appearance.
4. Set up the content experiment in Google Analytics. It should give you a code snippet to page into the original page in your experiment. Paste that into the 'Content experiment page' that targets your original page and republish it.