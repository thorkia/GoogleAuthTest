using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthTest.WepAPI.Controllers
{
	[Route("authentication/external")]
	public class ExternalController : ControllerBase
	{

		[AllowAnonymous]
		[HttpGet("google-login")]
		public IActionResult GoogleLogin(string returnUrl)
		{
			returnUrl = "/Index";

			var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleLoginCallback), new { returnUrl }) };

			return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
		}

		[AllowAnonymous]
		[HttpPost("google-callback")]
		public async Task<IActionResult> GoogleLoginCallback(string returnUrl = null, string remoteError = null)
		{
			//Here we can retrieve the claims - never gets hit
			var result = await HttpContext.AuthenticateAsync("AuthScheme");

			if (!result.Succeeded)
			{
				return BadRequest();
			}

			var claimsIdentity = new ClaimsIdentity("APIScheme");

			claimsIdentity.AddClaims(result.Principal.Claims);

			await HttpContext.SignInAsync("APIScheme", 
			                        new ClaimsPrincipal(claimsIdentity),
			                        new AuthenticationProperties {IsPersistent = true});


			return LocalRedirect(returnUrl);
		}

	}
}