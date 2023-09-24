using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Nullref.FullStackDemo.API.Widget.Controllers;
using Nullref.FullStackDemo.CommonModels;
using Nullref.FullStackDemo.Database;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Nullref.FullStackDemo.Tests
{
    public class ValidatorTests
    {
        /// <summary>
        /// Find all REST controllers and verify that each follows best practices.
        /// </summary>
        [Fact]
        public void ValidateControllers()
        {
            var allControllers = typeof(WidgetController).Assembly.GetTypes()
                .Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract)
                .ToList();

            var errors = new List<string>();
            foreach (var controller in allControllers)
            {
                //Get the controller routes
                var controllerRouteAttribute = controller.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute), true).Single() as Microsoft.AspNetCore.Mvc.RouteAttribute;
                var controllerRoutes = (controllerRouteAttribute?.Template + string.Empty).Split('/').Where(x => x.Contains("{"))
                                .Select(x => x.Replace("{", string.Empty).Replace("}", string.Empty))
                                .ToList();

                var allMethods = controller.GetMethods().Where(x => x.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute), true).Any()).ToList();

                //Check "NoReturn" methods
                var noReturns = allMethods.Where(x => x.Name.Contains("NoReturn")).ToList();
                foreach (var mm in noReturns)
                {
                    if (mm.ReturnType != typeof(System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult>))
                    {
                        errors.Add($"'{controller.Name}.{mm.Name}' is marked 'NoReturn' but its return type is not 'Task<ActionResult>'.");
                    }
                }

                //If return nothing should be marked "NoReturn"
                foreach (var mm in allMethods.Where(x => !x.Name.Contains("NoReturn") && !x.Name.StartsWith("Queue")).ToList())
                {
                    if (mm.ReturnType == typeof(System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult>))
                    {
                        //Skip 'Accepted', but look at POST/PUT/GET
                        var httpItem = mm.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute), true).Single() as Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute;
                        if (!mm.Name.Contains("Accepted") &&
                            (httpItem.HttpMethods.Contains("POST") ||
                            httpItem.HttpMethods.Contains("PUT") ||
                            httpItem.HttpMethods.Contains("GET")))
                        {
                            errors.Add($"'{controller.Name}.{mm.Name}' return nothing but is not marked 'NoReturn'.");
                        }
                    }
                }

                //Check missing swagger description for 'Main' controllers
                //Sub controllers will have SwaggerOperation maps to main controllers and can be skipped
                var swaggerAttribute = controller.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerTagAttribute), true).FirstOrDefault();
                if (swaggerAttribute == null)
                {
                    var operationAttributes = allMethods.Where(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true).Any())
                        .SelectMany(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true))
                        .Select(x => x as Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute)
                        .Where(x => x.Tags?.Count() > 0)
                        .ToList();
                    if (operationAttributes.Count != allMethods.Count)
                        errors.Add($"'{controller.Name}' controller is missing a {nameof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute)}.");
                }
                else
                {
                    var operationAttributes = allMethods.Where(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true).Any())
                        .SelectMany(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true))
                        .Select(x => x as Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute)
                        .Where(x => x.Tags?.Count() > 0)
                        .ToList();
                    if (operationAttributes.Count == allMethods.Count)
                        errors.Add($"'{controller.Name}' controller has no end point methods.");
                }

                //Check that every endpoint method has a description
                var totalOperationAttributes = allMethods.Where(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true).Any())
                    .SelectMany(x => x.GetCustomAttributes(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute), true))
                    .Select(x => x as Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute)
                    .ToList();
                if (totalOperationAttributes.Count < allMethods.Count)
                    errors.Add($"'{controller.Name}' controller is missing a {nameof(Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute)}.");

                foreach (var method in allMethods)
                {
                    //Find the Http method attribute
                    var httpItem = method.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute), true).Single() as Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute;
                    var parameters = method.GetParameters();
                    foreach (var pp in parameters)
                    {
                        //All params are required
                        var atts = pp.GetCustomAttributes(true);
                        if (!atts.Any(x => x is RequiredAttribute))
                        {
                            errors.Add($"'{controller.Name}.{method.Name}.{pp.Name}' is missing a {nameof(RequiredAttribute)}.");
                        }

                        //All params must be explicitly From* something
                        if (!atts.Any(x => x is Microsoft.AspNetCore.Mvc.FromBodyAttribute ||
                                x is Microsoft.AspNetCore.Mvc.FromFormAttribute ||
                                x is Microsoft.AspNetCore.Mvc.FromQueryAttribute ||
                                x is Microsoft.AspNetCore.Mvc.FromRouteAttribute ||
                                x is Microsoft.AspNetCore.Mvc.FromServicesAttribute))
                        {
                            errors.Add($"'{controller.Name}.{method.Name}.{pp.Name}' is missing a FromBody, FromForm, FromQuery, FromRoute, or FromServices.");
                        }
                    }

                    //Verify that each template item matches a parameter
                    if (!string.IsNullOrEmpty(httpItem.Template))
                    {
                        var arr = httpItem.Template.Split('/').Where(x => x.Contains("{")).ToList();
                        foreach (var tt in arr)
                        {
                            var name = tt.Replace("{", string.Empty).Replace("}", string.Empty);
                            if (!parameters.Any(x => x.Name == name))
                                errors.Add($"'{controller.Name}.{method.Name}' is missing the parameter {tt} defined as a template.");
                        }
                    }

                    //Ensure each "FromRoute" parameter matches a template
                    var routeParams = parameters.Where(x => x.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromRouteAttribute), true).Any()).ToList();
                    foreach (var pp in routeParams)
                    {
                        var methodRoutes = (httpItem?.Template + string.Empty).Split('/').Where(x => x.Contains("{"))
                                .Select(x => x.Replace("{", string.Empty).Replace("}", string.Empty))
                                .ToList();

                        //Look in the controller and method templates
                        if (!methodRoutes.Concat(controllerRoutes).Any(x => x == pp.Name))
                            errors.Add($"'{controller.Name}.{method.Name}.{pp.Name}' is marked as FromRoute but there is no Http template.");
                    }
                }
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateUIModels()
        {
            //All IModel objects should not have any reference to any database object
            //NOTE: This just checks the top level for now, not nested
            var types = typeof(WidgetController).Assembly.GetTypes()
                .Where(p => typeof(IModel).IsAssignableFrom(p) && !p.IsAbstract)
                .ToList();

            var collector = new List<Type>();
            var errors = WalkTree(types, collector);

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));

            static List<string> WalkTree(List<Type> types, List<Type> collector)
            {
                //The namespace from the data layer should never be found in UI objects
                var badNamespace = typeof(DataContext).Namespace;
                var result = new List<string>();
                foreach (var t2 in types.Where(x => !x.IsValueType))
                {
                    var t = t2;
                    //If an generic list then check the generic type
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                        t = t.GetGenericArguments()[0];

                    if (!collector.Contains(t) && !t.IsValueType)
                    {
                        collector.Add(t);
                        if (t.Namespace.StartsWith(badNamespace))
                            result.Add($"'{t.Name}' model cannot contain data layer references.");

                        var childTypes = t
                            .GetProperties()
                            .Where(x => !x.PropertyType.IsValueType && x.PropertyType != typeof(string))
                            .Select(x => x.PropertyType)
                            .ToList();

                        foreach (var child in childTypes)
                        {
                            if (!collector.Contains(child))
                            {
                                result.AddRange(WalkTree(childTypes, collector));
                            }
                        }
                    }
                }
                return result;
            }
        }

        [Fact]
        public void ValidateControllerInputOutput()
        {
            var allControllers = typeof(WidgetController).Assembly.GetTypes()
                .Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract)
                .ToList();

            var errors = new List<string>();
            foreach (var controller in allControllers)
            {
                var allMethods = controller.GetMethods().Where(x => x.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute), true).Any()).ToList();
                foreach (var method in allMethods)
                {
                    //Check parameters
                    var parameters = method.GetParameters();
                    foreach (var pp in parameters)
                    {
                        if (pp.ParameterType.IsClass && !pp.ParameterType.IsAssignableTo(typeof(IModel)))
                        {
                            errors.Add($"'{controller.Name}.{method.Name}' method '{pp.Name}' parameter must implement '{nameof(IModel)}'");
                        }
                    }

                    //Check output
                    if (method.ReturnType.IsClass)
                    {
                        //get type an unwrap if Task
                        var t = method.ReturnType;
                        if (t.IsAssignableTo(typeof(Task)))
                        {
                            t = t.GenericTypeArguments[0];
                            if (t.IsGenericType)
                            {
                                if (t.GetGenericTypeDefinition() == typeof(ActionResult<>))
                                    t = t.GenericTypeArguments[0];
                            }
                        }
                        //if it is just plain 'ActionResult' then there is no returned type, so skip
                        //This happens for the Update/Delete methods
                        if (t != typeof(ActionResult) && !t.IsAssignableTo(typeof(IModel)))
                            errors.Add($"'{controller.Name}.{method.Name}' method return type must implement '{nameof(IModel)}'");
                    }
                }
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateUIRequired()
        {
            //UI models with nullable values types like "int?" cannot be marked with the RequiredAttribute
            var types = typeof(WidgetController).Assembly.GetTypes()
                .Where(p => typeof(IModel).IsAssignableFrom(p) && !p.IsAbstract)
                .ToList();

            var errors = new List<string>();
            foreach (var t in types)
            {
                foreach (var p in t.GetProperties())
                {
                    //Has a [Required] attribute but is nullable value type: Error
                    if (p.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)))
                    {
                        if (p.PropertyType.IsValueType && p.PropertyType.Name == "Nullable`1")
                            errors.Add($"'{t.Name}.{p.Name}' is marked with '{nameof(RequiredAttribute)}' and cannot be nullable.");
                    }

                    //Does not have a [Required] attribute but is a value type so should be marked: Error
                    if (!p.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)))
                    {
                        if (p.PropertyType.IsValueType && p.PropertyType.Name != "Nullable`1")
                            errors.Add($"'{t.Name}.{p.Name}' is a non-nullable value type and should be marked with '{nameof(RequiredAttribute)}'.");
                    }
                }
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateReadOnlyTypesHaveNoPropertySetters()
        {
            var assem = typeof(WidgetController).Assembly;
            var types = assem.GetTypes()
                .Where(p => !p.IsAbstract)
                .ToList();

            var errors = new List<string>();
            foreach (var t in types.Where(x => x.GetCustomAttributes(true).Any(x => x is ReadOnlyAttribute)))
            {
                //Only check public properties declared on the actual object NOT inherited
                var props = t
                    .GetProperties(System.Reflection.BindingFlags.Public |
                                   System.Reflection.BindingFlags.Instance |
                                   System.Reflection.BindingFlags.DeclaredOnly);

                //The total property count should equal the non-public setter count
                if (props.Count(x => x.GetSetMethod() == null) != props.Count())
                    errors.Add($"'{t.Name}' is marked with '{nameof(ReadOnlyAttribute)}' so all properties must not have a public setter.");
            }

            if (errors.Any())
                Assert.Fail($"Errors: {errors.Count}\r\n" + string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateNoDateTimes()
        {
            var types = typeof(WidgetController).Assembly.GetTypes()
                .Where(p => typeof(IModel).IsAssignableFrom(p));

            var errors = new List<string>();
            foreach (var t in types)
            {
                errors.AddRange(t
                    .GetProperties()
                    .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?))
                    .ToList()
                    .Select(p => $"'{t.Name}.{p.Name}' cannot be a '{nameof(DateTime)}'. " +
                        "All dates must be 'LocalDate' or 'LocalDateTime'.")
                );
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateDateTimeHasValidator()
        {
            var types = typeof(WidgetController).Assembly.GetTypes()
                .Where(p => typeof(IModel).IsAssignableFrom(p));

            var errors = new List<string>();
            foreach (var t in types)
            {
                var props = t.GetProperties()
                    .Where(x => x.PropertyType == typeof(LocalDate) ||
                            x.PropertyType == typeof(LocalDate?) ||
                            x.PropertyType == typeof(LocalDateTime?) ||
                            x.PropertyType == typeof(LocalDateTime?));
                foreach (var p in props)
                {
                    if (!p.CustomAttributes
                        .Any(x => x.AttributeType.IsAssignableTo(typeof(RangeAttribute))))
                    {
                        errors.Add($"Model date property '{t.Name}.{p.Name}'" +
                            $" is missing '{nameof(RangeAttribute)}'");
                    }
                }
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }

        [Fact]
        public void ValidateStringsMaxLength()
        {
            var types = typeof(WidgetController).Assembly.GetTypes()
                .Where(p => typeof(IModel).IsAssignableFrom(p));

            var errors = new List<string>();
            foreach (var t in types)
            {
                foreach (var p in t.GetProperties()
                    .Where(x => x.PropertyType == typeof(string)))
                {
                    if (!p.CustomAttributes
                        .Any(x => x.AttributeType.IsAssignableTo(typeof(MaxLengthAttribute))))
                    {
                        errors.Add($"Model property '{t.Name}.{p.Name}'" +
                            $" is missing '{nameof(MaxLengthAttribute)}'");
                    }
                }
            }

            if (errors.Any())
                Assert.Fail(string.Join("\r\n", errors));
        }
    }
}
