using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrintMatic.Extensions
{
	public class ValidationAuthorization : IAuthorizationMiddlewareResultHandler
	{
		private readonly AuthorizationMiddlewareResultHandler authorizationMiddleware=new AuthorizationMiddlewareResultHandler();
		public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
		{
			if (authorizeResult.Challenged)
			{
				context.Response.StatusCode=(int)HttpStatusCode.Unauthorized;
				await context.Response.WriteAsJsonAsync(new { Message = "غير مسموح لك بالوصول، يرجى تسجيل الدخول" });
				return;
			}
			if (authorizeResult.Forbidden)
			{
				context.Response.StatusCode = (int)HttpStatusCode.OK;
				await context.Response.WriteAsJsonAsync(new { Message = "غير مسموح لك بالوصول، يرجى تسجيل الدخول" });
				return;
			}
			await authorizationMiddleware.HandleAsync(next, context, policy, authorizeResult);
		}
	}
}
