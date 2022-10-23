using System;
using System.Runtime.Serialization;

namespace DateBaseServices.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при взаимодействии с базой данных.
    /// </summary>
    public class DbServiceException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public DbServiceException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public DbServiceException(string message) : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public DbServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc cref="Exception"/>
        protected DbServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}