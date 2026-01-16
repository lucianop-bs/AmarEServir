using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;

namespace AmarEServir.Core.Filters
{
    public class ApiResultFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var resultValue = objectResult.Value;
                if (resultValue == null) return;

                var type = resultValue.GetType();

                if (type.Name.StartsWith("Result"))
                {
                    var isSuccess = (bool)type.GetProperty("IsSuccess")?.GetValue(resultValue)!;

                    if (isSuccess)
                    {

                        var data = type.GetProperty("Value")?.GetValue(resultValue);
                        context.Result = new OkObjectResult(data);
                    }
                    else
                    {

                        var errors = type.GetProperty("Errors")?.GetValue(resultValue) as IEnumerable;
                        var firstError = errors?.Cast<object>().FirstOrDefault();

                        if (firstError != null)
                        {

                            var message = firstError.GetType().GetProperty("Message")?.GetValue(firstError)?.ToString();

                            var errorStatus = (int)(firstError.GetType().GetProperty("Status")?.GetValue(firstError) ?? 400);

                            context.Result = new ObjectResult(new
                            {
                                status = errorStatus,
                                message = message
                            })
                            {
                                StatusCode = errorStatus
                            };
                        }
                    }
                }
            }
        }
    }
}