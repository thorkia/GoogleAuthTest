using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthTest.WepAPI.Controllers
{
	[Route("authentication/external")]
	public class ExternalController : ControllerBase
	{

		[AllowAnonymous]
		[HttpGet("google-login")]
		public IActionResult GoogleLogin(string? returnUrl)
		{
			return new ChallengeResult(GoogleDefaults.AuthenticationScheme);
		}

	}
}