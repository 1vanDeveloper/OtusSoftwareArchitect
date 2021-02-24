using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

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
            return new List<ApiResource>
            {
                new ApiResource("account", "Account Service"),
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
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
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
                new ApiScope("account", "Account Service")
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
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "Otus App SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    //AllowedCorsOrigins = {$"{clientsUrl["Spa"]}"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,   
                        "account"
                    },
                },
                new Client
                {
                    ClientId = "account",
                    ClientName = "Account Service",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        "account"
                    }
                }
            };
        }
    }
}