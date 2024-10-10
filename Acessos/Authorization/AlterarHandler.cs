using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Acessos.Authorization;

public class AlterarHandler : AuthorizationHandler<LerRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LerRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "permissao" && c.Value == "alterar"))
        {
            context.Succeed(requirement);
        }
        else
        {
            throw new UnauthorizedAccessException("Usuário não tem permissão para usar esse método");
        }

        return Task.CompletedTask;
    }
}
