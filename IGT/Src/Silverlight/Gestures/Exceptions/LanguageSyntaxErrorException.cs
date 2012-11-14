using System;

namespace TouchToolkit.GestureProcessor.Exceptions
{
    public class LanguageSyntaxErrorException : Exception
    {
        public LanguageSyntaxErrorException(string message)
            : base(message)
        { }
    }
}
