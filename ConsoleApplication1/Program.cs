using ConsoleApplication1.Models;
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
        
        public static void Main(string[] args)
        {
            var JsonMapperService = new JsonMapper();

            var example = new Example1(1, 1.0, new List<string> { "a", "b" }, new List<int> { 27, 1, 2, 3, 4, 5, 6, 50, 7, 8 }, new List<double> { 1.1, 1.2 }, new Dictionary<string, string> { { "asd", "123" }, { "def", "456" } }, new List<Example2> { new Example2(7, new Dictionary<string, string> { { "fgh", "789" } }, new Dictionary<int, string> { { 3, "dasdas" } }) });
            var json = JsonConvert.SerializeObject(example);
         
            var path1 = "example2[0].dict[\"fgh\"].Value()";
            var testeSetVariable = JsonMapperService.SetJsonProperty(json, path1, "Teste");
          
            var path = "example2[0].dictint.elementAt(0).Value()";

            var result = JsonMapperService.GetJsonProperty(json, path);
            
        }

        
    }
}
