using System;
using System.Linq;
using Chubberino.Common.Extensions;
using ValueOf;

namespace Chubberino.Common.ValueObjects
{
    public sealed class LowercaseString : ValueOf<String, LowercaseString>
    {
        public const String FormatExceptionMesage = "String value \"{0}\" must be all lowercase.";

        protected override void Validate()
        {
            if (Value.Any(Char.IsUpper))
            {
                throw new FormatException(FormatExceptionMesage.Format(Value));
            }
        }
    }
}
