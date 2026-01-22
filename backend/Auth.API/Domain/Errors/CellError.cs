using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain.Errors
{
    public class CellError
    {
        public static Error NameRequired => new(
            "Cell.NameRequired",
            "O nome da célula é obrigatório.",
            ErrorType.Validation);
        public static Error NotFound => new(
            "Cell.NotFound",
            "A Célula informada não foi encontrada.",
            ErrorType.NotFound);

        public static Error LeaderInvalid => new(
            "Cell.LeaderInvalid",
            "O ID do líder informado é inválido.",
            ErrorType.Validation);
        public static Error InvalidNameLength => new(
            "Cell.InvalidNameLength",
            "O nome deve ter entre 3 e 50 caracteres.",
            ErrorType.Validation);

        public static Error IdRequired => new(
            "Cell.IdRequired",
            "O Id da célula é obrigatório.",
            ErrorType.Validation);

        public static Error AlreadyLeadingCell => new(
            "Cell.AlreadyLeadingCell",
            "O líder informado já está liderando uma célula.",
            ErrorType.Validation);

        public static Error LeaderNotFound => new(
            "Cell.LeaderNotFound",
            "O líder não encontrado.",
            ErrorType.Conflict);

        public static Error NameAlreadyExists => new(
            "Cell.NameAlreadyExists",
            "Já existe uma célula cadastrada com este nome.",
            ErrorType.Conflict);
        public static Error LeaderRoleRequired => new(
            "Cell.LeaderRoleRequired",
            "Somente usuários com perfil de Líder podem gerenciar uma célula.",
            ErrorType.Validation);

    }
}

