using AmarEServir.Core.Results.Errors;

namespace Auth.API.Domain.Errors
{
    public static class AuthErrors
    {
        public static Error InvalidCredentials => new(
            "Auth.InvalidCredentials",
            "Email ou senha inválidos.",
             ErrorType.Validation
          );

        public static Error EmailRequired => new(
            "Auth.EmailRequired",
            "Email deve ser preenchido.",
            ErrorType.Validation
         );

        public static Error InvalidEmail => new(
          "User.InvalidEmail",
          "O formato do e-mail é inválido.",
          ErrorType.Validation);

        public static Error InvalidToken => new(
            "Auth.InvalidToken",
            "Token inválido ou expirado.",
            ErrorType.Unauthorized
        );

        public static Error AccessDenied => new(
            "Auth.AccessDenied",
            "Acesso negado. Você não tem permissão para acessar este recurso.",
            ErrorType.Unauthorized
        );
        public static Error TokenRefreshRequired => new(
          "Auth.TokenRefreshRequired",
          "Nenhum usuario com refresh token encontrado.",
          ErrorType.Unauthorized
      );
        public static Error TokenRefreshRevoked => new(
        "Auth.TokenRefreshRevoked",
        "Token revogado.",
        ErrorType.Unauthorized
    );
        public static Error NotAuthenticated => new(
            "Auth.NotAuthenticated",
            "Você precisa estar autenticado para acessar este recurso.",
            ErrorType.Unauthorized
        );

        public static Error PasswordRequired => new(
            "User.PasswordRequired",
            "A senha deve ser preenchida.",
            ErrorType.Validation);

        public static Error WeakPassword => new(
            "User.WeakPassword",
            "A senha deve ter pelo menos 6 caracteres.",
            ErrorType.Validation);
    }
}