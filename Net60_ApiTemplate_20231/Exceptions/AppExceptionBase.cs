namespace Net60_ApiTemplate_20231.Exceptions
{
    public class AppExceptionBase : Exception
    {
        public string ObjectTypeName { get; set; }
        public string Keys { get; set; }
    }
}