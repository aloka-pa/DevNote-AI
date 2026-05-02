using DevNoteAI.Application.Contracts;
using DevNoteAI.Application.Rewrite;
using DevNoteAI.Infrastructure.Options;
using DevNoteAI.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevNoteAI.WebApi;

public static class CompositionRoot
{
    public static IServiceCollection AddDevNoteAi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRewriteTextUseCase, RewriteTextUseCase>();
        services.Configure<AiOptions>(configuration.GetSection(AiOptions.SectionName));
        services.AddHttpClient<IAiRewriteService, OpenAiRewriteService>();
        return services;
    }
}
