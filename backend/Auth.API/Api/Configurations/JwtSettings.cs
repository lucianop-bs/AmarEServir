namespace Auth.API.Api.Configurations;

/// <summary>
/// Classe que representa as configurações do JWT.
/// Será preenchida automaticamente pelo ASP.NET a partir do appsettings.json
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Nome da seção no appsettings.json
    /// </summary>
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// Chave secreta usada para assinar o token.
    /// IMPORTANTE: Em produção, use uma chave forte e guarde em local seguro!
    /// Mínimo recomendado: 256 bits (32 caracteres)
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Quem emitiu o token (sua API).
    /// Exemplo: "https://api.amarservir.com.br"
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Para quem o token é destinado (seu frontend).
    /// Exemplo: "https://app.amarservir.com.br"
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tempo de expiração do token em minutos.
    /// Exemplo: 60 = 1 hora, 1440 = 24 horas
    /// </summary>
    public int ExpirationInMinutes { get; set; } = 60;
}