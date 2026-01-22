using Auth.API.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.API.Api.Configurations;

/// <summary>
/// Classe de extensão para configurar autenticação JWT.
/// Separamos em uma classe própria para manter o código organizado.
/// </summary>
public static class AuthConfig
{
    /// <summary>
    /// Configura todos os serviços necessários para autenticação JWT.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ═══════════════════════════════════════════════════════════════
        // PASSO 1: Ler as configurações do appsettings.json
        // ═══════════════════════════════════════════════════════════════
        //
        // O método Configure<T>() faz o "binding" entre a seção do JSON
        // e a classe JwtSettings. Isso permite injetar IOptions<JwtSettings>
        // em qualquer lugar.
        //
        var jwtSettingsSection = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSettingsSection);

        // Pega os valores para usar na configuração abaixo
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>()!;
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        // ═══════════════════════════════════════════════════════════════
        // PASSO 2: Configurar o esquema de autenticação
        // ═══════════════════════════════════════════════════════════════
        //
        // AddAuthentication() registra o serviço de autenticação.
        // JwtBearerDefaults.AuthenticationScheme = "Bearer"
        //
        // Quando você usa [Authorize], o ASP.NET vai:
        // 1. Pegar o header "Authorization: Bearer <token>"
        // 2. Extrair o token
        // 3. Validar usando as regras abaixo
        //
        services.AddAuthentication(options =>
        {
            // Define que o esquema padrão é JWT Bearer
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // ═══════════════════════════════════════════════════════════
            // PASSO 3: Configurar validação do token
            // ═══════════════════════════════════════════════════════════
            //
            // TokenValidationParameters define COMO o token será validado.
            // É como as instruções para o porteiro verificar o crachá.
            //
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // ✅ Validar a assinatura do token
                // Garante que o token foi emitido pela sua API (não foi forjado)
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                // ✅ Validar o emissor (Issuer)
                // Garante que o token veio da fonte correta
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                // ✅ Validar a audiência (Audience)
                // Garante que o token é destinado a este cliente
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                // ✅ Validar o tempo de expiração
                // Rejeita tokens expirados
                ValidateLifetime = true,

                // ⏰ Tolerância de tempo (para diferenças de relógio entre servidores)
                // Padrão é 5 minutos, mas você pode ajustar
                ClockSkew = TimeSpan.Zero  // Sem tolerância (mais seguro)
            };

            // ═══════════════════════════════════════════════════════════
            // PASSO 4: Eventos de autenticação (opcional, para debug/log)
            // ═══════════════════════════════════════════════════════════
            options.Events = new JwtBearerEvents
            {
                // Executado quando a autenticação falha
                OnAuthenticationFailed = context =>
                {
                    // Útil para logar erros de autenticação
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        // Token expirado
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },

                // Executado quando o token é validado com sucesso
                OnTokenValidated = context =>
                {
                    // Aqui você pode fazer validações adicionais
                    // Por exemplo: verificar se o usuário ainda está ativo no banco
                    return Task.CompletedTask;
                }
            };
        });

        // ═══════════════════════════════════════════════════════════════
        // PASSO 5: Configurar autorização
        // ═══════════════════════════════════════════════════════════════
        //
        // AddAuthorization() configura as políticas de autorização.
        // Você pode criar políticas personalizadas aqui.
        //
        services.AddAuthorization(options =>
        {
            // Política para Leaders (líderes de célula)
            options.AddPolicy("LeaderOnly", policy =>
                policy.RequireRole("Leader"));

            // Política para admins
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
            // Política para administração (admin + líder)
            options.AddPolicy("ManagementOnly", policy =>
                policy.RequireRole("Admin", "Leader"));

        });

        // ═══════════════════════════════════════════════════════════════
        // PASSO 6: Registrar o serviço de geração de token
        // ═══════════════════════════════════════════════════════════════
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}