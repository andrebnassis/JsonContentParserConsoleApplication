using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication1.Services
{
    public class JsonMapper : IJsonMapper
    {
        public const string OperationPattern = @"(.+)(\-\-|\+\+)$";
        public const string ArrayPattern = @"(.*)\[(.*)\]";
        public const string FunctionPattern = @"(.*\()(.*)(\))";

        public  object GetJsonProperty(string variableValue, string property)
        {
            try
            {
                // If there's a propertyName, attempts to parse the value as JSON and retrieve the value from it.
                var json = MapJsonProperty(variableValue, property);
                return JsonConvert.SerializeObject(json);

            }
            catch (JsonException)
            {
                return null;
            }
        }

        public  object SetJsonProperty(string variableValue, string property, string newValue)
        {
            var path = GetJsonPath(variableValue, property);
            JToken obj = JObject.Parse(variableValue);
            JToken token = obj.SelectToken(path);
            token.Replace(newValue);
            return obj;

        }

        private  JToken MapJsonProperty(string variableValue, string property)
        {
            JToken json = null;
            try
            {
                // If there's a propertyName, attempts to parse the value as JSON and retrieve the value from it.
                var propertyNames = property.Split(new string[] { ".", }, StringSplitOptions.RemoveEmptyEntries);
                propertyNames = propertyNames.Select(c => c.Trim()).ToArray();

                json = JObject.Parse(variableValue);


                var operation = "";
                foreach (var s in propertyNames)
                {

                    var matches = Regex.Matches(s, OperationPattern);
                    operation = matches.Count > 0 ? operation = ((matches[0]).Groups[2]).Value : "";

                    var input = string.IsNullOrEmpty(operation) ? s.Trim() : s.Replace(operation, "").Trim();


                    if (IsAnArray(input))
                    {
                        json = ProcessArrayJson(json, input);
                    }
                    else if (IsAFunction(input))
                    {
                        json = ProcessFunctionJson(json, input);
                    }
                    else
                    {
                        json = json[input];
                    }

                    if ("++".Equals(operation))
                    {
                        int number = 0;
                        var isNumber = int.TryParse(json.ToString(), out number);
                        if (isNumber)
                        {
                            number++;
                            json = number;
                        }
                    }

                    if ("--".Equals(operation))
                    {
                        int number = 0;
                        var isNumber = int.TryParse(json.ToString(), out number);
                        if (isNumber)
                        {
                            number--;
                            json = number;
                        }


                    }


                    if (json == null) return null;
                }

            }
            catch (JsonException)
            {
                return null;
            }

            return json;




        }

        private  string GetJsonPath(string variableValue, string property)
        {
            try
            {
                var json = MapJsonProperty(variableValue, property);
                return json.Path;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private  bool IsAFunction(string input)
        {
            return Regex.IsMatch(input, FunctionPattern);
        }

        private  JToken ProcessFunctionJson(JToken json, string function)
        {
            var matches = Regex.Matches(function, FunctionPattern);
            var parameter = ((matches[0]).Groups[2]).Value;
            function = Regex.Replace(function, FunctionPattern, "$1$3");



            try
            {
                switch (function.ToLower())
                {
                    case "first()":
                        return json.FirstOrDefault();
                    case "value()":
                        try
                        {
                            return ((JProperty)json).Value;
                        }
                        catch (Exception e)
                        {
                            return json;
                        }
                    case "key()":
                        {
                            try
                            {
                                return ((JProperty)json).Name;
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    case "last()":
                        return json.LastOrDefault();
                    case "min()":
                        return json.Min();
                    case "max()":
                        return json.Max();
                    case "count()":
                        return json.Count();
                    case "elementat()":
                        return json.ElementAt(int.Parse(parameter));
                        return null;
                    case "random()":
                        return json.ElementAt(new Random().Next(0, json.Count()));
                    default:
                        return json;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool IsAnArray(string input)
        {
            return Regex.IsMatch(input, ArrayPattern) ? true : false;
        }

        private JToken ProcessArrayJson(JToken json, string propertyName)
        {
            var matches = Regex.Matches(propertyName, ArrayPattern);
            json = json[matches[0].Groups[1].Value];
            if (!matches[0].Groups[2].Value.Contains("\""))
            {
                json = json[int.Parse(matches[0].Groups[2].Value)];

            }
            else
            {
                json = json[matches[0].Groups[2].Value.Replace("\"", "")];
            }


            return json;
        }
    }
}
