namespace Auth.API.Application.Users.Dtos
{
    public record class AddressRequestDto(
        string? Rua,
        string? Quadra,
        string? Numero,
        string? Bairro,
        string? Cidade,
        string? Estado,
        string? Complemento,
        string? Cep
    );
}
