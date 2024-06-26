namespace Net60_ApiTemplate_20231.Exceptions
{
    public class InvalidGUIDException : AppExceptionBase
    {
        public InvalidGUIDException(string objectTypeName, string keys)
        {
            ObjectTypeName = objectTypeName;
            Keys = keys;
        }

        public override string Message => $"Object [{ObjectTypeName}] ({Keys}) guid is not valid.";
    }
}