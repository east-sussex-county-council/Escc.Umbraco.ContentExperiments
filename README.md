# Escc.Umbraco.ContentExperiments

This project allows you to manage custom styles and scripts for Google Analytics content experiments in Umbraco. Each Umbraco page in a content experiment has a corresponding page using the 'Content experiment page' document type, and these are kept under a common 'Content experiments' document type which uses a list view.

The 'Content experiment page' document type has three fields: 

- Content experiment code, which is only used on page 1 of an experiment
- Content experiment scripts, which is a list of script URLs which should be loaded to customise the page under test, one URL per line
- Content experiment styles, which is a list of stylesheets which should be loaded to customise the page under test, one URL per line

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

## Using NuGet to include these templates in Umbraco

This project references the [Umbraco CMS NuGet package](https://www.nuget.org/packages/UmbracoCms) and is a working Umbraco installation, but this is just for testing the templates in a development environment. In production this project is published as a NuGet package to our private feed, and consumed by our main Umbraco application. We use [NuBuild](https://github.com/bspell1/NuBuild) to make creating the NuGet package really easy, and [reference our private feed using a nuget.config file](http://blog.davidebbo.com/2014/01/the-right-way-to-restore-nuget-packages.html).

When you install the package into another Umbraco project it copies in the views and references the DLL output. These are excluded from the consuming project's git respository by its `.gitignore` file. 

However, when the consuming project is deployed NuGet restore ensures the DLL reference still works, but it doesn't restore the views because [package restore doesn't restore content files](http://jeffhandley.com/archive/2013/12/09/nuget-package-restore-misconceptions.aspx). To work around this, the NuGet package includes a `.targets` file. NuGet automatically includes this as an MSBuild step in the `.csproj` file of the consuming project, which allows the `.targets` file to copy the views into the project at build time.

## Development setup steps

1. Install [NuBuild](https://github.com/bspell1/NuBuild)
2. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
3. Build the solution
4. Grant modify permissions to the application pool account on the web root folder and all children
7. In `~\web.config` set the `UmbracoConfigurationStatus` and `umbracoDbDSN`, or run the Umbraco installer.
8. At a command line, run the following two commands to add the document types to Umbraco (substitute the hostname and port where you set up this project):

		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/ContentExperiments/CreateUmbracoSupportingTypes?token=
		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/ContentExperiments/CreateUmbracoDocumentTypes?token=