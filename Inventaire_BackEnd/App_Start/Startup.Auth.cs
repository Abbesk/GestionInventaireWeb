
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Owin.Security.Jwt;

namespace Inventaire_BackEnd
{

    public partial class Startup
    {

        public void ConfigureAuth(IAppBuilder app)
        {
            var jwtOptions = new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_Here")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            };
            app.UseJwtBearerAuthentication(jwtOptions);
        }
    }

}
