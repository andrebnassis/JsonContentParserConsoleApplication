namespace ConsoleApplication1.Services
{
    public interface IJsonMapper
    {
        object GetJsonProperty(string variableValue, string property);
        object SetJsonProperty(string variableValue, string property, string newValue);
    }
}