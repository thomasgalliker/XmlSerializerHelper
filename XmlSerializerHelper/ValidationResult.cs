using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization.Exceptions;

namespace System.Xml.Serialization
{
    /// <summary>
    /// The validation result indicates success/failure of an XSD schema validation.
    /// </summary>
    public class ValidationResult
    {
        public ValidationResult(IEnumerable<XsdValidationException> errors)
        {
            this.Errors = errors.ToList();
        }

        public IEnumerable<XsdValidationException> Errors { get; }

        public bool IsValid
        {
            get
            {
                return !this.Errors.Any();
            }
        }
    }
}