using DevNoteAI.Application.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace DevNoteAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRewriteTextUseCase, RewriteTextUseCase>();
        return services;
    }
}
