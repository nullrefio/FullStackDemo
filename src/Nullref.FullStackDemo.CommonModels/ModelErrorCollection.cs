namespace Nullref.FullStackDemo.CommonModels
{
    /// <summary>
    /// This is used to validate models and add a collection of errors
    /// If any error exist at the end of valdidation, an exception is thrown
    /// </summary>
    public class ModelErrorCollection
    {
        private const string ErrorHeader = "Error";
        private readonly Dictionary<string, IEnumerable<FieldErrorModel>> _items = new();

        public void AddError(string message)
        {
            AddError(string.Empty, message);
        }

        public void AddError(string? objectName, string? message, int errorNumber = 0)
        {
            // adding with empty objectname name indicates an object-level error, rather than field-level
            objectName ??= string.Empty;
            message ??= string.Empty;

            lock (_items)
            {
                if (!_items.Keys.Contains(objectName))
                    _items.Add(objectName, new List<FieldErrorModel>());
                (_items[objectName] as List<FieldErrorModel>)?.Add(new FieldErrorModel { Message = message, ErrorNumber = errorNumber });
            }
        }

        public void ThrowIfErrors()
        {
            lock (_items)
            {
                if (_items.Values.Any())
                {
                    var copy = new Dictionary<string, IEnumerable<FieldErrorModel>>(_items);
                    _items.Clear();
                    throw new RequestValidationException(copy);
                }
            }
        }

        /// <summary>
        /// This is a convenience method to call the above
        /// </summary>
        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static Exception ThrowError(string objectName, string message)
        {
            var errors = new ModelErrorCollection();
            errors.AddError(objectName, message);
            errors.ThrowIfErrors();
            //Never hit but needed to use Throw before method so compiler knows always exception
            throw new ApplicationException();
        }

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static Exception ThrowError(string message, int errorNumber = 0)
        {
            var errors = new ModelErrorCollection();
            errors.AddError(ErrorHeader, message, errorNumber);
            errors.ThrowIfErrors();
            //Never hit but needed to use Throw before method so compiler knows always exception
            throw new ApplicationException();
        }

        public static void ThrowErrorNotFound(string? objectName = null)
            => throw new EntityNotFoundException(objectName);

        public static void ThrowErrorExists(string? objectName = null)
            => throw new EntityExistsException(objectName);

        #region AddErrorIfNotFound

        public T AddErrorIfNotFound<T>(T entity, string? objectName, string? message = null)
        {
            if (entity == null)
            {
                if (string.IsNullOrEmpty(message))
                    message = $"The '{objectName}' value was not found.";
                AddError(objectName, message);
            }
            return entity;
        }

        public void AddErrorIfNotFound(bool predicate, string? objectName, string? message = null)
        {
            if (predicate)
            {
                if (string.IsNullOrEmpty(message))
                    message = $"The '{objectName}' value was not found.";
                AddError(objectName, message);
            }
        }

        #endregion

        public void AddErrorIfNotUnique(bool predicate, string objectName)
        {
            if (predicate)
            {
                AddError(objectName, $"The '{objectName}' must be unique.");
            }
        }

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static void ThrowInUse() => throw new EntityInUseException();
    }
}
