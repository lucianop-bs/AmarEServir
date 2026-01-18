using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AmarEServir.Core.Filters;

public class ApiResultFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Intercepta o resultado antes de virar JSON
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var type = objectResult.Value.GetType();

            // Só processa se for uma de suas classes de Result
            if (type.Name.Contains("Result"))
            {
                // Mapeamento dinâmico para não errar o nome da propriedade
                var isSuccessProp = type.GetProperty("IsSuccess") ?? type.GetProperty("Success");
                var dataProp = type.GetProperty("Value") ?? type.GetProperty("Data");
                var errorsProp = type.GetProperty("Errors");

                if (isSuccessProp != null)
                {
                    bool isSuccess = (bool)isSuccessProp.GetValue(objectResult.Value)!;

                    if (isSuccess)
                    {
                        // SUCESSO: Entrega apenas o conteúdo (ID, Nome, etc)
                        objectResult.Value = dataProp?.GetValue(objectResult.Value);
                    }
                    else
                    {
                        // FALHA: Entrega a lista de erros e força o Status 400
                        objectResult.Value = errorsProp?.GetValue(objectResult.Value);
                        objectResult.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
            }
        }
    }
}