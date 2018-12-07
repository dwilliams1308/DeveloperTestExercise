using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileData;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileDataUnitTests
{
    [TestClass]
    public class FileDataUnitTests
    {
        [TestMethod]
        [DataRow("-v", "c:\test.doc")]
        [DataRow("--v", "c:\test.doc")]
        [DataRow("/v", "c:\test.doc")]
        [DataRow("--version", "c:\test.doc")]
        public void ShouldSucceedForCorrectVersionArgs(string arg, string filePath)
        {
            //Arrange
            string[] args = new string[] { arg, filePath };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            List<string> actionResults = fileDataProcessor.RunActions();

            //Assert
            Assert.IsTrue(actionResults.Count == 1); //Only one result expected
            string[] versionNumberSections = actionResults[0].Split('.');
            int numericalVersionNumberSection = 0;
            foreach (var versionNumberSection in versionNumberSections)
            {
                Assert.IsTrue(int.TryParse(versionNumberSection, out numericalVersionNumberSection));
            }
        }

        [TestMethod]
        [DataRow("-s", "c:\test.doc")]
        [DataRow("--s", "c:\test.doc")]
        [DataRow("/s", "c:\test.doc")]
        [DataRow("--size", "c:\test.doc")]
        public void ShouldSucceedForCorrectSizeArgs(string arg, string filePath)
        {
            //Arrange
            string[] args = new string[] { arg, filePath };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            List<string> actionResults = fileDataProcessor.RunActions();

            //Assert
            Assert.IsTrue(actionResults.Count == 1); //Only one result expected
            long numericalVersionNumberSection = 0;
            Assert.IsTrue(long.TryParse(actionResults[0], out numericalVersionNumberSection));
        }

        [TestMethod]
        [DataRow("-S", "c:\test.doc")]
        [DataRow("123", "c:\test.doc")]
        [DataRow("---v", "c:\test.doc")]
        [DataRow("abc", "c:\test.doc")]
        public void ShouldFailForInCorrectFileActionArgs(string arg, string filePath)
        {
            //Arrange
            string[] args = new string[] { arg, filePath };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            bool isValid = fileDataProcessor.HasValidArgs(out Collection<string> validationMessages);

            //Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationMessages.Count == 1);
            Assert.IsTrue(validationMessages[0] == $"{arg} is an unknown action or command");
        }


        [TestMethod]
        public void ShouldFailDueTooZeroArgs()
        {
            //Arrange
            string[] args = new string[] {};
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            bool isValid = fileDataProcessor.HasValidArgs(out Collection<string> validationMessages);

            //Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationMessages.Count == 1);
            Assert.IsTrue(validationMessages[0] == "Too few arguments.");
        }

        [TestMethod]
        [DataRow("--version")]
        [DataRow("--size")]
        [DataRow("--v")]
        [DataRow("-s")]
        public void ShouldFailForTooFewArgs(string arg)
        {
            //Arrange
            string[] args = new string[] { arg };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            bool isValid = fileDataProcessor.HasValidArgs(out Collection<string> validationMessages);

            //Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationMessages.Count == 1);
            Assert.IsTrue(validationMessages[0] == "Too few arguments.");
        }

        [TestMethod]
        [DataRow("--version", "--size", "c:\test.txt")]
        public void ShouldFailForTooManyArgs(string arg1, string arg2, string arg3)
        {
            //Arrange
            string[] args = new string[] { arg1, arg2, arg3 };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            bool isValid = fileDataProcessor.HasValidArgs(out Collection<string> validationMessages);

            //Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationMessages.Count == 1);
            Assert.IsTrue(validationMessages[0] == "Too many arguments.");
        }

        [TestMethod]
        [DataRow("--Version")]
        [DataRow("--Size")]
        [DataRow("--g")]
        [DataRow("-a")]
        public void ShouldFailForTooFewArgsAndInvalidArgs(string arg)
        {
            //Arrange
            string[] args = new string[] { arg };
            FileDataProcessor fileDataProcessor = new FileDataProcessor(args);

            //Act
            bool isValid = fileDataProcessor.HasValidArgs(out Collection<string> validationMessages);

            //Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationMessages.Count == 2);
            Assert.IsTrue(validationMessages[0] == "Too few arguments.");
            Assert.IsTrue(validationMessages[1] == $"{arg} is an unknown action or command");
        }
    }
}
