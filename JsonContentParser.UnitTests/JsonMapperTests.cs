using System;
using System.Collections.Generic;
using ConsoleApplication1.Models;
using ConsoleApplication1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace JsonContentParser.UnitTests
{
    [TestClass]
    public class JsonMapperTests
    {
        [TestMethod]
        public void WhenPassSomePath_ShouldReturnItsCorrespondantValue()
        {
            var path18 = "example2[0].dictint.elementAt(0).Value()";

            //Arrange
            var JsonMapperService = new JsonMapper();
            var example = new Example1(1, 1.0, new List<string> { "a", "b" }, new List<int> { 27, 1, 2, 3, 4, 5, 6, 50, 7, 8 }, new List<double> { 1.1, 1.2 }, new Dictionary<string, string> { { "asd", "123" }, { "def", "456" } }, new List<Example2> { new Example2(7, new Dictionary<string, string> { { "fgh", "789" } }, new Dictionary<int, string> { { 3, "dasdas" } }) });
            var json = JsonConvert.SerializeObject(example);

            //Act
            var result = JsonMapperService.GetJsonProperty(json, path18);

            //Assert
            Assert.AreEqual("", result);

        }
    }
}
