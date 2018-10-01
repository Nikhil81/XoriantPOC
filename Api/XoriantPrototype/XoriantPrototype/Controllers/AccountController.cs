using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using XoriantPrototype.Models.Account;
using XoriantPrototype.ViewModels.Account;

namespace XoriantPrototype.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        #region Private Variables
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserRegistrationRepository userRegistrationRepository;

        private readonly string USER_EXSITS = "USER_EXISTS";
        private readonly string ERROR_SAVING = "ERROR_SAVING";
        #endregion

        #region Ctor
        public AccountController(IHostingEnvironment hostingEnvironment, IUserRegistrationRepository userRegistrationRepository)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.userRegistrationRepository = userRegistrationRepository;
        }

        #endregion

        #region Actions

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromForm]UserInformation model)
        {
            if (ModelState.IsValid)
            {
                if (userRegistrationRepository.GetUserByUsername(model.UserName))
                {
                    bool success = userRegistrationRepository.SaveUserRegistrationInfo(model);
                    if (success)
                    {
                        return Ok();
                    }
                }

                return BadRequest(USER_EXSITS);
            }
            else
            {
                return BadRequest(ERROR_SAVING);
            }
        }

        #endregion
    }
}
