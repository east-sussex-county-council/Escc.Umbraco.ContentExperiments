using System;
using System.Collections.Generic;
using System.Linq;

namespace Escc.Umbraco.ContentExperiments
{
    /// <summary>
    /// Parser to turn a text field with one URL per line into an IList&lt;Uri&gt;
    /// </summary>
    public class ListOfUrlsFieldParser
    {
        /// <summary>
        /// Parses the urls.
        /// </summary>
        /// <param name="valueToParse">A text string containing URLs on separate lines.</param>
        /// <param name="urls">The urls.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void ParseUrls(string valueToParse, IList<Uri> urls)
        {
            if (urls == null) throw new ArgumentNullException("urls");

            var urlLines = valueToParse.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in urlLines)
            {
                // Only allow scripts and styles from the folder where Umbraco saves them, to minimise XSS risk
                var trimmed = line.Trim();
                var isAllowedFolder = (trimmed.StartsWith("/scripts/", StringComparison.OrdinalIgnoreCase) || trimmed.StartsWith("/css/", StringComparison.OrdinalIgnoreCase));
                var isAllowedExtension = (trimmed.EndsWith(".js", StringComparison.OrdinalIgnoreCase) || trimmed.EndsWith(".css", StringComparison.OrdinalIgnoreCase));
                if (isAllowedFolder && isAllowedExtension)
                {
                    urls.Add(new Uri(trimmed, UriKind.Relative));
                }
            }
        }
    }
}