using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "ShoppingCartClientId",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =  { "ShoppingCartApi" }
                }
            };

        public static IEnumerable<ApiScope> Scopes =>
          new List<ApiScope>()
          {
              new ApiScope("ShoppingCartApi", "Shopping Cart Api")
          };

        public static IEnumerable<ApiResource> Resources =>
            new List<ApiResource>();

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>();

        public static IEnumerable<TestUser> TestUsers =>
          new List<TestUser>();
    }
}
