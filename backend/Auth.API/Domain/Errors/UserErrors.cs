using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain.Errors;

public static class UserErrors
{

    public static class Account
    {
        public static Error EmailRequired => new("User.EmailRequired", "O e-mail deve ser preenchido.", ErrorType.Validation);
        public static Error InvalidEmail => new("User.InvalidEmail", "O formato do e-mail é inválido.", ErrorType.Validation);
        public static Error EmailAlreadyExists => new("User.EmailAlreadyExists", "Este e-mail já está cadastrado em nossa base.", ErrorType.Conflict);

        public static Error PasswordRequired => new("User.PasswordRequired", "A senha deve ser preenchida.", ErrorType.Validation);
        public static Error WeakPassword => new("User.WeakPassword", "A senha deve ter pelo menos 6 caracteres.", ErrorType.Validation);

        public static Error RoleInvalid => new("User.RoleInvalid", "Tipo de perfil inválido.", ErrorType.Validation);
        public static Error InvalidForRole => new("User.InvalidForRole", "Você não é líder de nenhuma célula.", ErrorType.Validation);
        public static Error NotFound => new("User.NotFound", "Usuário não encontrado.", ErrorType.NotFound);
    }

    public static class Profile
    {
        public static Error NameRequired => new("User.NameRequired", "O nome completo deve ser preenchido.", ErrorType.Validation);
        public static Error NameLength => new("User.NameLength", "O nome deve conter de 3 a 50 caracteres.", ErrorType.Validation);

        public static Error PhoneRequired => new("User.PhoneRequired", "O telefone deve ser preenchido.", ErrorType.Validation);
        public static Error PhoneInvalid => new("User.PhoneInvalid", "O telefone deve conter de 11 a 13 caracteres.", ErrorType.Validation);
    }

    public static class Address
    {
        public static Error AddressRequired => new("Address.Required", "O endereço deve ser preenchido.", ErrorType.Validation);

        public static Error CepRequired => new("Address.CepRequired", "O CEP é obrigatório.", ErrorType.Validation);
        public static Error CepFormat => new("Address.CepFormat", "O CEP deve estar no formato 00000000.", ErrorType.Validation);

        public static Error EstadoRequired => new("Address.EstadoRequired", "O estado deve ser preenchido.", ErrorType.Validation);
        public static Error EstadoInvalid => new("Address.EstadoInvalid", "O estado deve conter 2 caracteres.", ErrorType.Validation);

        public static Error RuaRequired => new("Address.RuaRequired", "A rua deve ser preenchida.", ErrorType.Validation);
        public static Error RuaInvalid => new("Address.RuaInvalid", "A rua deve conter entre 2 e 100 caracteres.", ErrorType.Validation);

        public static Error BairroRequired => new("Address.BairroRequired", "O bairro é obrigatório.", ErrorType.Validation);
        public static Error BairroInvalid => new("Address.BairroInvalid", "O bairro deve conter entre 2 e 200 caracteres.", ErrorType.Validation);

        public static Error CidadeRequired => new("Address.CidadeRequired", "A cidade deve ser preenchida.", ErrorType.Validation);
        public static Error CidadeInvalid => new("Address.CidadeInvalid", "A cidade deve conter entre 2 e 100 caracteres.", ErrorType.Validation);

        public static Error NumeroRequired => new("Address.NumeroRequired", "O número deve ser preenchido.", ErrorType.Validation);
        public static Error NumeroLimit => new("Address.NumeroLimit", "O número deve conter entre 1 e 20 caracteres.", ErrorType.Validation);
    }
}