using DevNoteAI.Application.Contracts;
using DevNoteAI.Infrastructure.Options;
using DevNoteAI.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevNoteAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AiOptions>(configuration.GetSection(AiOptions.SectionName));
        services.AddHttpClient<IAiRewriteService, OpenAiRewriteService>();
        return services;
    }
}
