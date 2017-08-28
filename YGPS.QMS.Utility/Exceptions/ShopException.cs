using System;
using System.Runtime.Serialization;

namespace InstagramPhotos.Utility.Exceptions
{
    [Serializable]
    public class ShopException : Exception
    {
        public ShopException()
        {
        }

        public ShopException(string message) : base(message)
        {
        }

        public ShopException(string message, string reason)
            : base(message)
        {
            ReasonPhrase = reason;
        }

        public ShopException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ShopException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public string ReasonPhrase { get; set; }
    }
}