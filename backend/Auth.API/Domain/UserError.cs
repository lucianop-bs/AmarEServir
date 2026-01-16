using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain
{
    public class UserError
    {
        public static Error NameRequired => new("User.InvalidName", "O nome completo deve ser preenchido.", ErrorType.Validation);

        public static Error InvalidEmail => new("User.InvalidEmail", "O formato do e-mail é inválido.", ErrorType.Validation);

        public static Error WeakPassword => new("User.WeakPassword", "A senha deve ter pelo menos 6 caracteres.", ErrorType.Validation);

        public static Error EmailAlreadyExists => new("User.EmailAlreadyExists", "Este e-mail já está cadastrado em nossa base.", ErrorType.Conflict);

        public static Error NotFound => new("User.NotFound", "Usuário não encontrado.", ErrorType.NotFound);

        public static Error TypeInvalid => new("User.Type", "Tipo de usuario invalido", ErrorType.Validation);

        public static Error PhoneInvalid => new("User.Phone", "O telefone deve conter de 11 a 13 caracteres.", ErrorType.Validation);

        public static Error CepRequired => new("Address.Cep", "O CEP é obrigatório.", ErrorType.Validation);

        public static Error CepFormat => new("Address.Cep", "O CEP deve estar no formato 00000000.", ErrorType.Validation);

        public static Error EstadoInvalid => new("Address.Estado", "O estado deve conter 2 caracteres.", ErrorType.Validation);

        public static Error RuaInvalid => new("Address.Rua", "A rua deve conter no máximo 100 caracteres.", ErrorType.Validation);
        public static Error CidadeInvalid => new("Address.Cidade", "A cidade deve conter no máximo 100 caracteres.", ErrorType.Validation);
        public static Error BairroInvalid => new("Address.Bairro", "O bairro deve conter no máximo 100 caracteres.", ErrorType.Validation);

        public static Error BairroRequired => new("Address.Bairro", "O bairro é obrigatório.", ErrorType.Validation);

        public static Error CidadeRequired => new("Address.Cidade", "A cidade é obrigatória.", ErrorType.Validation);

        public static Error NumeroLimit => new("Address.Lote", "O lote deve conter no máximo 20 caracteres.", ErrorType.Validation);
        public static Error NumeroRequired => new("Address.Numero", "O número deve ser preenchido", ErrorType.Validation);

        public static Error QuadraLimit => new("Address.Quadra", "A quadra deve conter no máximo 20 caracteres.", ErrorType.Validation);

        public static Error ComplementoLimit => new("Address.Complemento", "O complemento deve conter no máximo 100 caracteres.", ErrorType.Validation);

        public static Error RoleInvalid => new("User.Role", "Tipo inválido", ErrorType.Validation);

        public static Error InvalidForRole => new("User.Role", "Você não é lider de nenhuma célula.", ErrorType.Validation);

        public static Error AddressRequired => new("User.Address", "O endereço deve ser preenchido.", ErrorType.Validation);
        public static Error NameLength => new("User.Name", "O nome deve conter de 3 a 50 caracteres.", ErrorType.Validation);
        public static Error PhoneRequired => new("User.Phone", "O telefone deve ser preenchido.", ErrorType.Validation);

    }
}
