using AmarEServir.Core.Results.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AmarEServir.Core.Filters;

public class ApiResultFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is IResultBase result)
        {
            var resultType = result.GetType();

            if (result.IsSuccess)
            {
           
                var dataProp = resultType.GetProperty("Value");
                objectResult.Value = dataProp?.GetValue(result);
            }
            else
            {
                
                objectResult.Value = result.Errors;

          
                var statusProp = resultType.GetProperty("Status") ?? resultType.GetProperty("StatusCode");

                if (statusProp != null)
                {
                    
                    objectResult.StatusCode = (int)statusProp.GetValue(result)!;
                }
                else
                {
                   
                    objectResult.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
        }
    }
}