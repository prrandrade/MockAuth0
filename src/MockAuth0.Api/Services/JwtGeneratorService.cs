using Microsoft.IdentityModel.Tokens;
using MockAuth0.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MockAuth0.Api.Services
{
    public interface IJwtGeneratorService
    {
        RSA Rsa { get; }
        RsaSecurityKey PublicKey { get; }
        RsaSecurityKey PublicAndPrivateKey { get; }
        JsonWebKey Jwk { get; }
        string JwksStr { get; }
        JsonWebKeySet Jwks { get; }
        Dictionary<string, IList<JsonWebKey>> JwksDict { get; }
        IList<JsonWebKey> JwksList { get; }
        string GenerateToken(string email, string clientId, string organizationId, string nonce, List<KeyValuePair<string, string>> customClaims);
    }

    public class JwtGeneratorService : IJwtGeneratorService
    {
        public RSA Rsa { get; }
        public RsaSecurityKey PublicKey { get; }
        public RsaSecurityKey PublicAndPrivateKey { get; }
        public JsonWebKey Jwk { get; }
        public string JwksStr { get; }
        public JsonWebKeySet Jwks { get; }
        public Dictionary<string, IList<JsonWebKey>> JwksDict { get; }
        public IList<JsonWebKey> JwksList { get; }

        private readonly AddressesModel _addressesModel;

        public JwtGeneratorService(AddressesModel addressesModel)
        {
            Rsa = RSA.Create(2048);
            PublicKey = new(Rsa.ExportParameters(false))
            {
                KeyId = "keyId1"
            };
            PublicAndPrivateKey = new(Rsa.ExportParameters(true))
            {
                KeyId = "keyId1"
            };
            Jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(PublicKey);

            JwksList = new List<JsonWebKey>
            {
                Jwk
            };
            JwksDict = new()
            {
                { "keys", JwksList }
            };

            JwksStr = JsonExtensions.SerializeToJson(JwksDict);
            Jwks = new(JwksStr);

            _addressesModel = addressesModel;
        }

        public string GenerateToken(string email, string clientId, string organizationId, string nonce, List<KeyValuePair<string, string>> customClaims)
        {
            var credentials = new SigningCredentials(PublicAndPrivateKey, SecurityAlgorithms.RsaSsaPssSha256);

            var claims = new List<Claim>
            {
                new ("updated_at", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z"),
                new ("iss", _addressesModel.Issuer),
                new ("aud", clientId),
                new ("iat", ((long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString()),
                new ("exp", ((long)DateTime.UtcNow.AddDays(4).Subtract(DateTime.UnixEpoch).TotalSeconds).ToString()),
                new ("sub", Guid.NewGuid().ToString()),
                new ("sid", Guid.NewGuid().ToString()),
                new ("nonce", nonce),
                new ("org_id", organizationId),
                new ("name", email),
                new ("nickname", email.Split('@', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0]),
            };
            claims.AddRange(customClaims.Select(customClaim => new Claim(customClaim.Key, customClaim.Value)));

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(claims);

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
