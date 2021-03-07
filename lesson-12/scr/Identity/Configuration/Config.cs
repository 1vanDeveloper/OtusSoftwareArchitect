using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace Identity.Configuration
{
    /// <summary>
    /// Defined in your system configurations
    /// </summary>
    public static class Config
    {
        // 
        /// <summary>
        /// Api resource configurations define in your system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>()
            {
                new ApiResource("accountService", "Account Service")
                {
                    Scopes = { "accountService" }
                },
            };
        }

        /// <summary>
        /// Identity resources configurations define in your system
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Identity resources are data like user ID, name, or email address of a user
        /// see: http://docs.identityserver.io/en/release/configuration/resources.html
        /// </remarks>
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        /// <summary>
        /// Api scopes configurations define in your system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetScopes()
        {
            return new[]
            {
                new ApiScope("accountService", "Account Service")
            };
        }
            

        /// <summary>
        /// Client want to access resources (aka scopes)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "accountService",
                    ClientName = "Account Service",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowAccessTokensViaBrowser =true,
                    AllowedScopes =
                    {
                        //IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "accountService"
                    }
                },
                
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    IdentityTokenLifetime = 3600,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 30,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "accountService"
                    },
                    ClientSecrets = new []
                    {
                        new Secret
                        {
                            Value = "secret".ToSha256()
                        }
                    }
                }
            };
        }
    }
}