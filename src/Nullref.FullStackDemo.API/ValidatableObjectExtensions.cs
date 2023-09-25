using Nullref.FullStackDemo.CommonModels.Attributes;
using Nullref.FullStackDemo.CommonModels.Exceptions;

namespace Nullref.FullStackDemo.API
{
    public static class ValidatableObjectExtensions
    {
        public static T Validate<T>(this T item)
            where T : IValidatableObject
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            var validationResults = new List<ValidationResult>();
            // must pass validateAllProperties:true, otherwise only [Required] attribute is validated
            var isValid = true;
            var processedList = new List<object>();
            isValid &= Validate(item, validationResults, processedList);

            if (!isValid)
            {
                // project the validation results to the structure expected by the exception and ultimately the api result
                throw new RequestValidationException(validationResults
                    .Select(x => new
                    {
                        Path = string.Join(".", x.MemberNames),
                        x.ErrorMessage
                    })
                    .GroupBy(x => x.Path)
                    .ToDictionary(
                        x => x.Key,
                        x => x
                            .Select(y => new FieldErrorModel
                            {
                                Message = y.ErrorMessage
                            })));
            }
            return item;
        }

        private static bool Validate(IValidatableObject item, List<ValidationResult> validationResults, List<object> processedList)
        {
            processedList.Add(item);
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(item, context, results, validateAllProperties: true);
            validationResults.AddRange(results);

            var cachePropKey = $"Validate|{item.GetType().FullName}|properties";
            var propertiesAll = item.GetType().GetProperties();
            var propertiesAutoTrim = propertiesAll.Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(AutoTrimAttribute))).ToList();
            var propertiesAutoRequired = propertiesAll.Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(RequiredAttribute))).ToList();

            //For all string properties marked with 'AutoTrim' ensure that they are trimed
            //Do this before checking for the [Required] attribute as by default it
            //does not allow empty strings, so if the Trim creates an empty string, it is a validation error
            foreach (var prop in propertiesAutoTrim)
            {
                var v = prop.GetValue(item);
                if (v is string && v != null)
                {
                    var v1 = (string)v;
                    var v2 = v1.Trim();
                    if (v1 != v2)
                        prop.SetValue(item, v2);
                }
            }

            //Walk down the hierarchy
            foreach (var prop in propertiesAll)
            {
                var v = prop.GetValue(item);
                if (v != null)
                {
                    var t = v.GetType();
                    //If a validatable object then walk down its tree to validate
                    if (v is IValidatableObject v1 && !processedList.Contains(v1))
                    {
                        isValid &= Validate(v1, validationResults, processedList);
                    }
                    //If List<T> and T is IValidatableObject then loop through the list
                    else if (t.IsGenericType &&
                        t.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) &&
                        t.GenericTypeArguments.Length == 1 &&
                        t.GenericTypeArguments[0].IsAssignableTo(typeof(IValidatableObject)))
                    {
                        foreach (IValidatableObject child in v as System.Collections.IEnumerable)
                        {
                            isValid &= Validate(child, validationResults, processedList);
                        }
                    }
                }
            }

            //Check the [Required] attribute
            foreach (var prop in propertiesAutoRequired)
            {
                // Validate required dates (non-nullable and value = 1/1/1)
                // The required attribute alone does not handle this as it default to 1/1/1 if the property is completely missing
                if (prop.PropertyType == typeof(DateTime) && (DateTime)prop.GetValue(item) == default)
                {
                    validationResults.Add(new ValidationResult($"The {prop.Name} field is required.", new[] { prop.Name }));
                    isValid = false;
                }

                if (prop.PropertyType == typeof(NodaTime.LocalDate) && (NodaTime.LocalDate)prop.GetValue(item) == default)
                {
                    validationResults.Add(new ValidationResult($"The {prop.Name} field is required.", new[] { prop.Name }));
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
