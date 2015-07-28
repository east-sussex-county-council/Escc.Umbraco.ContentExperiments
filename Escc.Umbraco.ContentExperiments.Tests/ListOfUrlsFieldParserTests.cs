using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Escc.Umbraco.ContentExperiments.Tests
{
    [TestFixture]
    public class ListOfUrlsFieldParserTests
    {
        [Test]
        public void BlankLinesAreIgnored()
        {
            var fieldValue = Environment.NewLine + "/scripts/test.js" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "/scripts/test.js" + Environment.NewLine;
            var list = new List<Uri>();

            new ListOfUrlsFieldParser().ParseUrls(fieldValue, list);

            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void AbsoluteUrlsAreIgnored()
        {
            var fieldValue = "/scripts/test.js" + Environment.NewLine + "http://www.example.org/scripts/test.js";
            var list = new List<Uri>();

            new ListOfUrlsFieldParser().ParseUrls(fieldValue, list);

            Assert.AreEqual(1, list.Count);
        }

        [Test]
        public void DisallowedFoldersAreIgnored()
        {
            var fieldValue = "/scripts/test.js" + Environment.NewLine + "/css/test.css" + Environment.NewLine + "/other/other.css";
            var list = new List<Uri>();

            new ListOfUrlsFieldParser().ParseUrls(fieldValue, list);

            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void DisallowedExtensionsAreIgnored()
        {
            var fieldValue = "/scripts/test.js" + Environment.NewLine + "/css/test.css" + Environment.NewLine + "/other/other.txt";
            var list = new List<Uri>();

            new ListOfUrlsFieldParser().ParseUrls(fieldValue, list);

            Assert.AreEqual(2, list.Count);
        }
    }
}
