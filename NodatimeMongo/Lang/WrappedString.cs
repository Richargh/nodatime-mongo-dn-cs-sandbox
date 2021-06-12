using System.Collections.Generic;

namespace Richargh.Sandbox.NodatimeMongo.Lang
{
    /// <summary>
    /// This class makes strongly typed primitives possible.
    ///
    /// The exclamation marks are really ugly but I don't know how to do this better.
    /// I think the compiler assumes that WrappedPrimitive can be implemented in another project which does not have nullability as errors enabled.
    /// </summary>
    public abstract class WrappedString : ValueObject
    {
        public abstract string RawValue { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RawValue!;
        }
        
        public override string ToString()
        {
            return RawValue!.ToString()!;
        }
    }
}