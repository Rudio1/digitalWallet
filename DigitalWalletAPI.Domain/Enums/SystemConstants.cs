namespace DigitalWalletAPI.Domain.Enums
{
    public static class SystemConstants
    {
        #region WALLET
        public const string InitialBalanceParameter = "value_initial_wallet";
        #endregion

        #region JWT
        public const string JwtSecretKeyParameter = "jwt_secret_key";
        public const string JwtExpirationInMinutesParameter = "jwt_expiration_minutes";
        public const string JwtIssuerParameter = "jwt_issuer";
        public const string JwtAudienceParameter = "jwt_audience";
        #endregion
    }
} 