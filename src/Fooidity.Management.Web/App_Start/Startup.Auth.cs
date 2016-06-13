namespace Fooidity.Management.Web
{
    using System;
    using Autofac;
    using AzureIntegration.UserStore;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;
    using Microsoft.WindowsAzure;
    using Models;
    using Owin;
    using Owin.Security.Providers.GitHub;
    using Providers;


    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, () => WebAppContainer.Container.Resolve<UserManager<UserEntity>>()),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(WebAppContainer.Container.Resolve<UserManager<ApplicationUser>>);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<UserManager<ApplicationUser>, ApplicationUser>(TimeSpan.FromMinutes(30),
                            (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseOAuthBearerTokens(OAuthOptions);

            var gitHubOptions = new GitHubAuthenticationOptions
            {
                ClientId = "3f40d105ba3888d9e6a7",// CloudConfigurationManager.GetSetting("GitHub.ClientId"),
                ClientSecret = "b3f8a6578549ee0f02640664c6bcde9fdaa1ad9c"//CloudConfigurationManager.GetSetting("GitHub.ClientSecret"),
            };

            app.UseGitHubAuthentication(gitHubOptions);
        }
    }
}