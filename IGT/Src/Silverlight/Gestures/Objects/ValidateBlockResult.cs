using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class ValidateBlockResult
    {
        public Guid Id { get; private set; }
        public string GestureName { get; set; }
        public int ValidateBlockNo { get; set; }
        public ValidSetOfPointsCollection Data { get; set; }
        public string ValidateBlockName { get; set; }
        public List<Guid> AssociatedResults { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public ValidateBlockResult()
        {
            Id = Guid.NewGuid();
            AssociatedResults = new List<Guid>();
            TimeStamp = DateTime.Now;
        }
    }
}
