using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using MoravianStar_Demo.Common.Core.Constants;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Mobile.WebAPI.Infrastructure.HttpRequestInterceptors
{
    /// <summary>
    /// This HttpRequestInterceptor sets the client's Id in the global context upon the authentication.
    /// </summary>
    public class ClientContextHttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            var clientIdString = context.Request.Headers[HTTPHeaderConstants.ClientId].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(clientIdString))
            {
                requestBuilder.SetGlobalState(HTTPHeaderConstants.ClientId, clientIdString);
            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}