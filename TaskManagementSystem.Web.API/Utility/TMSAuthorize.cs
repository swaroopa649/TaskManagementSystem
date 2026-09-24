using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagementSystem.Web.API.Utility
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Method)]
    public class TMSAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null) return;

            if (!filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authTokens))
            {
                UnauthorizeRequest(filterContext, "Please Provide Authorization Token");
                return;
            }

            var rawToken = authTokens.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(rawToken))
            {
                UnauthorizeRequest(filterContext, "Please Provide Authorization Token");
                return;
            }

            string authToken = rawToken.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

            var claimsPrincipal = ValidateToken(filterContext.HttpContext, authToken);
            if (claimsPrincipal == null)
            {
                UnauthorizeRequest(filterContext, "Invalid or Expired Token");
                return;
            }

            // Set authenticated user context
            filterContext.HttpContext.User = claimsPrincipal;

            // Populate HttpContext.Items for quick downstream access
            filterContext.HttpContext.Items["AccessToken"] = authToken;
            filterContext.HttpContext.Items["UserId"] = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            filterContext.HttpContext.Items["Email"] = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            filterContext.HttpContext.Items["Role"] = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
            filterContext.HttpContext.Items["Name"] = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        }

        private ClaimsPrincipal? ValidateToken(HttpContext context, string token)
        {
            try
            {
                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                var jwtSettings = configuration.GetSection("Jwt");

                var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key missing");
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = !string.IsNullOrEmpty(issuer),
                    ValidIssuer = issuer,
                    ValidateAudience = !string.IsNullOrEmpty(audience),
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Validates signature, issuer, audience, and expiration simultaneously
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        private static void UnauthorizeRequest(AuthorizationFilterContext filterContext, string message)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            filterContext.Result = new JsonResult(new
            {
                Status = "Error",
                Message = message
            });
        }
    }
}