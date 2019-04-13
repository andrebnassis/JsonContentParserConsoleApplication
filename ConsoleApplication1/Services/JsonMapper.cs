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
        public const string ArrayPattern = "(?<name>(.*)?)\\[(?<index>(.*)?)\\]";
        public const string FunctionPattern = @"(.*\()(.*)(\))";

        public string GetJsonProperty(string variableValue, string property)
        {
            try
            {
                // If there's a propertyName, attempts to parse the value as JSON and retrieve the value from it.
                var result = MapJsonProperty(variableValue, property);
                var resultString = JsonConvert.SerializeObject(result);

                resultString = resultString.StartsWith("\"") ? resultString.Substring(1) : resultString;
                resultString = resultString.EndsWith("\"") ? resultString.Substring(0, resultString.Length - 1) : resultString;

                return resultString;

            }
            catch (JsonException)
            {
                return null;
            }
        }

        public string SetJsonProperty(string variableValue, string property, string newValue)
        {
            var path = GetJsonPath(variableValue, property);
            var obj = JObject.Parse(variableValue);
            if (!string.IsNullOrEmpty(path))
            {

                JToken token = obj.SelectToken(path);
                token.Replace(newValue);

            }
            else
            {
                if (CanSetVariableContent(variableValue, property, property.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).First()))
                {
                    var pathItems = property.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    if (pathItems.Count() == 1)
                    {
                        return newValue;
                    }
                    else
                    {
                        if (Regex.IsMatch(pathItems[pathItems.Count() - 2], ArrayPattern))
                        {
                            var match = Regex.Match(pathItems[pathItems.Count() - 2], ArrayPattern);
                            (obj.Property(match.Groups["name"].Value).Values().ElementAt(int.Parse(match.Groups["index"].Value)) as JObject).Add(pathItems[pathItems.Count() - 1], newValue); //Add when property does not exist! Think about the algorithm ñow.

                        }
                        else
                        {
                            obj.Add(pathItems.LastOrDefault(), newValue);
                        }
                    }
                }

                //Se não existe, cria o caminho.
                //Precisa melhorar, pois não está criando o caminho de acordo com o objeto passado, caso seja um objeto complexo. Ex: friendInfo.Name ou mais complexos.

            }
            return JsonConvert.SerializeObject(obj);
        }

        private bool CanSetVariableContent(string variableValue, string entirePath, string currentPath)
        {
            var lastpath = entirePath.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            var pathLeft = entirePath.Substring(currentPath.Length).Trim();
            if (pathLeft.StartsWith("."))
            {
                pathLeft = pathLeft.Substring(1);
            }


            if (string.IsNullOrEmpty(pathLeft) && !Regex.IsMatch(lastpath, ArrayPattern))
            {
                return true;
            }

            var path = GetJsonPath(variableValue, currentPath);
            var obj = JObject.Parse(variableValue);

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            currentPath += $".{pathLeft.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).First()}";
            return CanSetVariableContent(variableValue, entirePath, currentPath);
        }

        private JToken MapJsonProperty(string variableValue, string property)
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

        private string GetJsonPath(string variableValue, string property)
        {
            try
            {
                var json = MapJsonProperty(variableValue, property);
                return json.Path;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private bool IsAFunction(string input)
        {
            return Regex.IsMatch(input, FunctionPattern);
        }

        private JToken ProcessFunctionJson(JToken json, string function)
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
                        throw new Exception($"function {function} not recognized");
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
