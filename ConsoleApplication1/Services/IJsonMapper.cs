namespace ConsoleApplication1.Services
{
    public interface IJsonMapper
    {
        string GetJsonProperty(string variableValue, string property);
        string SetJsonProperty(string variableValue, string property, string newValue);
    }
}