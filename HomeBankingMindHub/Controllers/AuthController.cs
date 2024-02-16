using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        //Inyectamos el repositorio de Cliente para interactuar con la bdd
        private IClientRepository _clientRepository;
        public AuthController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Client client)
        {
            try
            {
                if (client.Password.IsNullOrEmpty())
                    return StatusCode(400, "Por favor completar el campo de la clave");

                if (client.Email.IsNullOrEmpty())
                    return StatusCode(400, "Por favor completar el campo de mail");

                Client user = _clientRepository.FindByEmail(client.Email);
                if (user == null)
                    return StatusCode(400, "Credenciales invalidas, por favor intente nuevamente");

                PasswordHasher<Client> passwordHasher = new();
                Console.WriteLine(passwordHasher.VerifyHashedPassword(user, user.Password, client.Password));

                if (passwordHasher.VerifyHashedPassword(user, user.Password, client.Password) == PasswordVerificationResult.Failed)
                    return StatusCode(400, "Credenciales invalidas, por favor intente nuevamente");

                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}