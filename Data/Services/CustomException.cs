using System.Runtime.Serialization;

namespace Data.Services
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        {

        }
    }
}