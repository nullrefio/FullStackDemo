namespace Nullref.FullStackDemo.CommonModels
{
    /// <summary>
    /// Default implementation of <see cref="IValidatableObject"/>, to support consistent validation. API
    /// models should extend and optionally override <see cref="Validate"/> if specific validations are
    /// not covered by annotations.
    /// </summary>
    public abstract class AbstractValidatableModel : IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
