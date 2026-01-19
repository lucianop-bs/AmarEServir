using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain.Errors
{
    public class CellError
    {
        public static Error InvalidName => new("Cell.InvalidName", "O nome da célula deve ser preenchido.", ErrorType.Validation);
        public static Error NotFound => new("Cell.NotFound", "A Célula informada não foi encontrada.", ErrorType.NotFound);
        public static Error LeaderRequired => new("Cell.LeaderRequired", "Usuário não é Líder.", ErrorType.Validation);
        public static Error LeaderInvalid => new("Cell.LeaderInvalid", "O ID do líder informado é inválido.", ErrorType.Validation);
        public static Error InvalidNameLength => new("Cell.InvalidNameLength", "O nome deve ter entre 3 e 50 caracteres.", ErrorType.Validation);

        public static Error IdRequired => new("Cell.IdRequired", "O Id da célula é obrigatório.", ErrorType.Validation);
    }
}

