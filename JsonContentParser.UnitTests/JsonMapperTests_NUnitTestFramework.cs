using ConsoleApplication1.Models;
using ConsoleApplication1.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonContentParser.UnitTests
{
    //Pre-requisites
    // install-package NUnit -Version 3.8.1
    // install-package NUnit3TestAdapter -Version 3.8.0

    [TestFixture]
    public class JsonMapperTests_NUnitTestFramework
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

        [TestCase("example2[0].dictint.elementAt(0).Value()", "dasdas")]
        [TestCase("stringlist[0]", "a")]
        public void NUnitTestFramework_WhenPassSomePath_ShouldReturnItsCorrespondantValue(string path, string expectedResult)
        {
            //Arrange
            var JsonMapperService = new JsonMapper();
            var json = JsonConvert.SerializeObject(exampleObj);

            //Act
            var result = JsonMapperService.GetJsonProperty(json, path);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }
    }
}
