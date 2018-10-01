using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XoriantPrototype.DAL.Registration;
using XoriantPrototype.Models.Account;
using XoriantPrototype.ViewModels.Account;

namespace XoriantPrototype.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        #region Private variable
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserRegistrationRepository userRegistrationRepository;
        private readonly SignInManager<StoreUser> singInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration config;
        private readonly string USER_EXSITS = "USER_EXISTS";
        private readonly string ERROR_SAVING = "ERROR_SAVING";
        private readonly string ERROR_CREATING_PROFILE = "ERROR_CREATING_PROFILE";
        private readonly string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        private readonly string SIGNIN_SUCCESSFUL = "SIGNIN_SUCCESSFUL";
        private readonly string SIGNOUT_SUCCESSFUL = "SIGNOUT_SUCCESSFUL";
        private readonly string FAILED_TO_GENERATE_TOKEN = "FAILED_TO_GENERATE_TOKEN";
        #endregion

        #region Ctor
        public AccountController(IHostingEnvironment hostingEnvironment, 
                        IUserRegistrationRepository userRegistrationRepository, 
                        SignInManager<StoreUser> singInManager, UserManager<StoreUser> userManager,
                        IConfiguration config)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.userRegistrationRepository = userRegistrationRepository;
            this.singInManager = singInManager;
            this.userManager = userManager;
            this.config = config;
        }
        #endregion

        #region Http Actions
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromForm]UserInformation model)
        {
            if (ModelState.IsValid)
            {
                if (userRegistrationRepository.GetUserByEmail(model.Email))
                {
                    bool success = userRegistrationRepository.SaveUserRegistrationInfo(model);
                    if (success)
                    {
                        var user = new StoreUser { UserName = model.UserName, Email = model.Email };
                        var result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                        return BadRequest(ERROR_CREATING_PROFILE);
                    }
                }
                return BadRequest(USER_EXSITS);
            }
            else
            {
                return BadRequest(ERROR_SAVING);
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await singInManager.PasswordSignInAsync(model.UserName, model.Password, model.RemmberMe, false);

                if (result.Succeeded)
                {
                    return Ok(SIGNIN_SUCCESSFUL);
                }
            }
            return BadRequest(INVALID_CREDENTIALS);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Logout()
        {
            await singInManager.SignOutAsync();
            return Ok(SIGNOUT_SUCCESSFUL);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var result = await singInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        //generate/Create the token
                        //step 1 clains
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        };

                        //to secure/encrypt the token
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                        //crea
                        var cread = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            config["Tokens:Issure"],
                            config["Tokens:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: cread
                            );

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo

                        };
                        return Created("", results);
                    }
                }
            }

            return BadRequest(FAILED_TO_GENERATE_TOKEN);
        }
        #endregion


    }
}
