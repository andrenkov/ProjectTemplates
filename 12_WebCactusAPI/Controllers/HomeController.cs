using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCactusAPI.Models;

namespace WebCactusAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        private string generatedToken = null;


        /// <summary>
        /// Constructor with DI
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tokenService"></param>
        /// <param name="userRepository"></param>
        public HomeController(IConfiguration config, ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _config = config;
        }
    }
}
