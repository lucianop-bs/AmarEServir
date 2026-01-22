namespace Auth.API.Domain
{
    public class RefreshToken
    {
        public string? Token { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Revoked { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;

        public RefreshToken()
        { }

        public RefreshToken(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
            Created = DateTime.UtcNow;
        }

        public void Revoke()
        {
            if (!IsRevoked)
            {
                Revoked = DateTime.UtcNow;
            }
        }
    }
}