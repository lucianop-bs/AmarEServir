using Auth.API.Domain.Enums;

namespace Auth.API.Application.Common.Models
{
    public record class UserResponse(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        AddressResponse Address,
        UserRole Role
        );
    public record class AddressResponse(
        string Rua,
        string Quadra,
        string Numero,
        string Bairro,
        string Cidade,
        string Estado,
        string Complemento,
        string Cep
        );
}