using Clientes.API.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clientes.API.API.Filters
{
    public class ValidarTokenAttribute : IAsyncActionFilter
    {
        private readonly IValidadorTokenRemoto _validador;

        public ValidarTokenAttribute(IValidadorTokenRemoto validador)
        {
            _validador = validador;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var authHeader = authHeaderValues.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader.Substring("Bearer ".Length).Trim()
                : authHeader.Trim();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool isValid;
            try
            {
                isValid = await _validador.ValidarAsync(token);
            }
            catch
            {
                isValid = false;
            }

            if (!isValid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
