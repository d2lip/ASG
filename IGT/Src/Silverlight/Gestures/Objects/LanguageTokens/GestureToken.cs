using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;


namespace TouchToolkit.GestureProcessor.Objects.LanguageTokens
{
    public class GestureToken : LanguageToken
    {
        private List<ValidateToken> _validationBlocks = new List<ValidateToken>();
        public List<ValidateToken> ValidateTokens
        {
            get
            {
                return _validationBlocks;
            }
            set
            {
                _validationBlocks = value;
            }
        }

        public List<ReturnToken> _returns = new List<ReturnToken>();
        public List<ReturnToken> Returns
        {
            get
            {
                return _returns;
            }
            set
            {
                _returns = value;
            }
        }
    }
}