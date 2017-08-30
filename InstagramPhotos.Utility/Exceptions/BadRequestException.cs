using System;
using System.Runtime.Serialization;

namespace InstagramPhotos.Utility.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, string reason)
            : base(message)
        {
            ReasonPhrase = reason;
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BadRequestException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public string ReasonPhrase { get; set; }
    }


    [Serializable]
    public class PaymentRequiredException : Exception
    {
        public PaymentRequiredException()
        {
        }

        public PaymentRequiredException(string message) : base(message)
        {
        }

        public PaymentRequiredException(string message, string reason)
            : base(message)
        {
            ReasonPhrase = reason;
        }

        public PaymentRequiredException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PaymentRequiredException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public string ReasonPhrase { get; set; }
    }
}