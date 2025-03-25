using System;

namespace SystemZarzadzaniaKontenerowcami
{
    public class ProductNotProvideException:Exception
    {
        public ProductNotProvideException()
        {
        }

        public ProductNotProvideException(string message) : base(message)
        {
        }

        public ProductNotProvideException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}