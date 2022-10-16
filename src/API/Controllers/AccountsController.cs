using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountsController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            var userBasket = await RetrieveBasket(loginDto.Username!, cancellationToken);
            var anonBasket = await RetrieveBasket(Request.Cookies[CookieConstants.KEY]!, cancellationToken);

            if (anonBasket != null)
            {
                if (userBasket != null) 
                { 
                    _context.Baskets.Remove(userBasket); 
                }
                anonBasket.BuyerId = user.UserName;
                Response.Cookies.Delete(CookieConstants.KEY);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateTokenAsync(user.UserName, user.Email),
                Basket = anonBasket != null ? _mapper.Map<BasketDto>(anonBasket) : _mapper.Map<BasketDto>(userBasket)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser 
            { 
                UserName = registerDto.Username, 
                Email = registerDto.Email 
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, RolesConstants.MEMBER);

            return StatusCode(201);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var userBasket = await RetrieveBasket(User?.Identity?.Name!, cancellationToken);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateTokenAsync(user.UserName, user.Email),
                Basket = _mapper.Map<BasketDto>(userBasket)
            };
        }

        private async Task<Basket?> RetrieveBasket(string buyerId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(buyerId))
            {
                Response.Cookies.Delete(CookieConstants.KEY);
                return null;
            }

            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId, cancellationToken);
        }
    }
}
