using ConsoleApplication1.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsonContentParser
{
    public class Program
    {
        public Program()
        {
            
        }
        [DataContract]
        public class Example1
        {
            public Example1(int id, double num, List<string> stringList, List<int> intList, List<double> doubleList, Dictionary<string, string> dict, List<Example2> example2)
            {
                Id = id;
                Num = num;
                StringList = stringList;
                IntList = intList;
                DoubleList = doubleList;
                Dict = dict;
                Example2 = example2;

            }

            [DataMember(Name = "id")]
            public int Id { get; set; }
            [DataMember(Name = "num")]
            public double Num { get; set; }
            [DataMember(Name = "stringlist")]
            public List<string> StringList { get; set; }
            [DataMember(Name = "intlist")]
            public List<int> IntList { get; set; }
            [DataMember(Name = "doublelist")]
            public List<double> DoubleList { get; set; }
            [DataMember(Name = "dict")]
            public Dictionary<string, string> Dict { get; set; }
            [DataMember(Name = "example2")]
            public List<Example2> Example2 { get; set; }
            //public Example2 Example2 { get; set; }

        }

        [DataContract]
        public class Example2
        {
            public Example2(int num, Dictionary<string, string> dict, Dictionary<int, string> dictInt)
            {
                Num = num;
                Dict = dict;
                DictInt = dictInt;

            }
            [DataMember(Name = "num")]
            public int Num { get; set; }
            [DataMember(Name = "dict")]
            public Dictionary<string, string> Dict { get; set; }
            [DataMember(Name = "dictint")]
            public Dictionary<int, string> DictInt { get; set; }

        }

        

        public static void Main(string[] args)
        {
            var JsonMapperService = new JsonMapper();

            var example = new Example1(1, 1.0, new List<string> { "a", "b" }, new List<int> { 27, 1, 2, 3, 4, 5, 6, 50, 7, 8 }, new List<double> { 1.1, 1.2 }, new Dictionary<string, string> { { "asd", "123" }, { "def", "456" } }, new List<Example2> { new Example2(7, new Dictionary<string, string> { { "fgh", "789" } }, new Dictionary<int, string> { { 3, "dasdas" } }) });
            var json = JsonConvert.SerializeObject(example);
            

            //var path1 = "example2[0].dict[\"fgh\"].Value()";
            //var testeSetVariable = SetJsonProperty(json, path1, "Teste");
          
            //var path3 = "example2[0].dict[\"fgh\"].Value()--";
            //var path4 = "dict.First().Value()++";
            //var path5 = "dict.Last().Value()++";
            //var path8 = "dict.Count()";
            //var path9 = "dict.Random()";
            //var path10 = "stringlist.First().Value()";
            //var path11 = "id.Value()";
            //var path12 = "intlist.Value()";
            //var path13 = "dict.First().Key()";
            //var path14 = "dict.Random().Key()";
            //var path15 = "stringlist.First()";
            //var path16 = "id.Value()";
            //var path17 = "intlist[0].Value()--";
            var path18 = "example2[0].dictint.elementAt(0).Value()";

            var result = JsonMapperService.GetJsonProperty(json, path18);

            //var result1 = GetJsonProperty(json, path5);
            //var result2 = GetJsonProperty(json, path6);
            //var result3 = GetJsonProperty(json, path7);
            //var result4 = GetJsonProperty(json, path8);
            //var result5 = GetJsonProperty(json, path9);
        }

        
    }
}
