using System;
namespace TouchToolkit.GestureProcessor.Exceptions
{
    public class InvalidDataSetException : Exception
    {
        public InvalidDataSetException(string message)
            : base(message)
        { }
    }
}
