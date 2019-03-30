using System;
using System.Collections.Generic;
using ConsoleApplication1.Models;
using ConsoleApplication1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace JsonContentParser.UnitTests
{


    [TestClass]
    public class JsonMapperTests_MSTestFramework
    {
        //{  
        //   "id":1,
        //   "num":1.0,
        //   "stringlist":[
        //      "a",
        //      "b"
        //   ],
        //   "intlist":[
        //      27,
        //      1,
        //      2,
        //      3,
        //      4,
        //      5,
        //      6,
        //      50,
        //      7,
        //      8
        //   ],
        //   "doublelist":[
        //      1.1,
        //      1.2
        //   ],
        //   "dict":{  
        //      "asd":"123",
        //      "def":"456"
        //   },
        //   "example2":[
        //      {  
        //         "num":7,
        //         "dict":{  
        //            "fgh":"789"
        //         },
        //         "dictint":{  
        //            "3":"dasdas"
        //         }
        //      }
        //   ]
        //}
        public Example1 exampleObj = new Example1(1, 1.0, new List<string> { "a", "b" }, new List<int> { 27, 1, 2, 3, 4, 5, 6, 50, 7, 8 }, new List<double> { 1.1, 1.2 }, new Dictionary<string, string> { { "asd", "123" }, { "def", "456" } }, new List<Example2> { new Example2(7, new Dictionary<string, string> { { "fgh", "789" } }, new Dictionary<int, string> { { 3, "dasdas" } }) });

        [DataTestMethod]
        [DataRow("id.Value()", "1")]
        [DataRow("stringlist[0]", "a")]
        [DataRow("stringlist.First()", "a")]
        [DataRow("stringlist.First().Value()", "a")]
        [DataRow("dict.First().Key()", "asd")]
        [DataRow("dict.First().Value()++", "124")]
        [DataRow("dict.Last().Value()++", "457")]
        [DataRow("dict.Count()", "2")]
        [DataRow("intlist.Value()", "[27,1,2,3,4,5,6,50,7,8]")]
        [DataRow("intlist[0].Value()--", "26")]
        [DataRow("example2[0].dict[\"fgh\"].Value()", "789")]
        [DataRow("example2[0].dict[\"fgh\"].Value()--", "788")]
        [DataRow("example2[0].dictint.elementAt(0).Value()", "dasdas")]
        public void MSTestFramework_WhenPassSomePath_ShouldReturnItsCorrespondantValue(string path, string expectedResult)
        {
            //Arrange
            var JsonMapperService = new JsonMapper();
            var json = JsonConvert.SerializeObject(exampleObj);

            //Act
            var result = JsonMapperService.GetJsonProperty(json, path);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [DataTestMethod]
        [DataRow("example2[0].dict[\"fgh\"].Value()", "Teste")]
        public void MSTestFramework_WhenSendSomePathAndValue_ShouldOverwriteItsValue(string path, string value)
        {
            //Arrange
            var JsonMapperService = new JsonMapper();
            var json = JsonConvert.SerializeObject(exampleObj);

            //Act
            var newJson = JsonMapperService.SetJsonProperty(json, path, value);

            var result = JsonMapperService.GetJsonProperty(newJson, path);

            //Assert
            Assert.AreEqual(value, result);

        }

        //TODO: unit test to test Random feature
        //[TestCase("dict.Random()", "")]
        // [TestCase("dict.Random().Key()", "")]
    }
}
