using System;
using System.Runtime.Serialization;

namespace Dal
{
    [Serializable]
    internal class DalException : Exception
    {
        public DalException()
        {
        }

        public DalException(string message) : base(message)
        {
        }

        public DalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}