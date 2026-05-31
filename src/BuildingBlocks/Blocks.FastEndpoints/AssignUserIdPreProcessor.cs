using Blocks.Core.Security;
using Blocks.Domain;
using FastEndpoints;

namespace Blocks.FastEndpoints
{
    public class AssignUserIdPreProcessor : IGlobalPreProcessor
    {
        public Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
        {
            if (context.Request is IAuditableAction action)
            {
                var claimsProvider = context.HttpContext.Resolve<IClaimsProvider>();
                var userId = claimsProvider.TryGetUserId();

                if (userId is not null)
                    action.CreatedById = userId.Value;
            }

            return Task.CompletedTask;
        }
    }
}
