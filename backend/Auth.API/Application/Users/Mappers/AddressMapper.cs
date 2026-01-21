using Auth.API.Application.Users.Dtos;
using Auth.API.Domain;

namespace Auth.API.Application.Users.Mappers
{
    public static class AddressMapper
    {
        public static AddressResponseDto ToResponseDto(this Address address)
        {
            if (address is null) return null!;

            return new AddressResponseDto(
                address.Rua,
                address.Quadra,
                address.Numero,
                address.Bairro,
                address.Cidade,
                address.Estado,
                address.Complemento,
                address.Cep
            );
        }

        public static Address ToDomain(this AddressRequestDto dto)
        {
            return new Address(
                dto.Rua ?? string.Empty,
                dto.Quadra ?? string.Empty,
                dto.Numero ?? string.Empty,
                dto.Bairro ?? string.Empty,
                dto.Cidade ?? string.Empty,
                dto.Estado ?? string.Empty,
                dto.Complemento ?? string.Empty,
                dto.Cep ?? string.Empty
            );
        }
    }
}
