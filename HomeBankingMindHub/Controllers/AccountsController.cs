using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services;
using HomeBankingMindHub.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using HomeBankingMindHub.Repositories.Implemetation;

namespace HomeBankingMindHub.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AccountsController : ControllerBase
        {
        private IAccountService _accountService;
            public AccountsController(IAccountService accountService)
            {
                _accountService = accountService;
            }

        [HttpGet]
            public IActionResult Get()
            {
                try
                {
                return Ok(_accountService.getAllAccounts());
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

        [HttpGet("{id}")]
            public IActionResult Get(long id)
            {
                try
                {
                    var accountDTO = _accountService.findById(id);
                    if (accountDTO == null)
                    {
                        return StatusCode(403, "No existe la cuenta");
                    }
                    return Ok(accountDTO);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }

