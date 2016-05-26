using System;

namespace Raza.Model
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string msg) : base(msg)
        {
        }
    }
}
