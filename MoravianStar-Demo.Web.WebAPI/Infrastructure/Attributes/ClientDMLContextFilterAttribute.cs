using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoravianStar_Demo.Persistence.DbContexts;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Infrastructure.Attributes
{
    /// <summary>
    /// This attribute is an action filter that sets the client's database connection string upon the authentication.
    /// </summary>
    public class ClientDMLContextFilterAttribute : IAsyncActionFilter
    {
        private readonly DataLayer_ClientDMLContext dbContext;
        private readonly IConfiguration configuration;

        public ClientDMLContextFilterAttribute(DataLayer_ClientDMLContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var clientIdString = context.HttpContext.Request.Headers["ClientId"];
            if (!string.IsNullOrEmpty(clientIdString))
            {
                var clientId = int.Parse(clientIdString);
                var connectionString = string.Format(configuration["ConnectionStrings:TestClient"], clientId);
                dbContext.Database.SetConnectionString(connectionString);
                try
                {
                    await next();
                }
                finally
                {
                    // Reset the connection string, just in case
                    dbContext.Database.SetConnectionString(null);
                }
            }
            else
            {
                await next();
            }
        }
    }
}