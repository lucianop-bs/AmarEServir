using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain
{
    public class CellError
    {
        public static Error InvalidName => new("Cell.InvalidName", "O nome da celula deve ser preenchido.", ErrorType.Validation);
        public static Error NotFound => new("Cell.NotFound", "Usuário não encontrado.", ErrorType.NotFound);
        public static Error LeaderRequired => new("Cell.InvalidName", "O nome da celula deve ser preenchido.", ErrorType.Validation);
        public static Error InvalidNameLength => new("Cell.InvalidName", "O nome deve ter entre 3 e 50 caracteres.", ErrorType.Validation);

    }
}

