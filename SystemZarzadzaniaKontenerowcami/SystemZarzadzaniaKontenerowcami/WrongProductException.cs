using System;

namespace SystemZarzadzaniaKontenerowcami
{
    public class WrongProductException : Exception
    {
            private string message;
            public WrongProductException()
            {
            }

            public WrongProductException(string message) : 
                base(message)
            {
            }

            public WrongProductException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        
    }
}