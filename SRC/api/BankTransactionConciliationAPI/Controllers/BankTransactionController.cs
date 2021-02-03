using AutoMapper;
using BankTransactionConciliationAPI.Extensions;
using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Models.Exceptions;
using BankTransactionConciliationAPI.Models.Request;
using BankTransactionConciliationAPI.Models.Response;
using BankTransactionConciliationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionConciliationAPI.Controllers
{
    [Route("bank-transactions")]
    [ApiController]
    public class BankTransactionController : BaseController
    {
        private readonly IBankTransactionService BankTransactionService;

        private readonly IMapper Mapper;

        public BankTransactionController(IMapper mapper, IBankTransactionService bankTransactionService)
        {
            this.BankTransactionService = bankTransactionService;
            this.Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagingContainerResponse<GetBankTransactionResponse>), 200)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Search([FromQuery] ListBankTransactionRequest request)
        {
            var filters = this.Mapper.Map<SearchPaging<BankTransactionFilters>>(request);

            var bankTransactionsResult = this.BankTransactionService.Search(filters);

            var response = this.Mapper.Map<PagingContainerResponse<GetBankTransactionResponse>>(bankTransactionsResult);

            return Ok(response);
        }

        [HttpPost("ofx")]
        [ProducesResponseType(typeof(List<GetBankTransactionResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/x-ofx")]
        [Produces("application/json")]
        public IActionResult CreateMany()
        {
            if (this.Request.ContentType != "application/x-ofx")
            {
                return new UnsupportedMediaTypeResult();
            }

            try
            {
                var ofx = this.Request.AsString();
                var bankTransactionsResult = this.BankTransactionService.CreateMany(ofx);
                var response = this.Mapper.Map<List<GetBankTransactionResponse>>(bankTransactionsResult);
                return Ok(response);
            }
            catch (InvalidOfxException)
            {
                var error = new ErrorResponse 
                { 
                    Message = "Invalid OFX file" 
                };
                return BadRequest(error);
            }
        }

        [HttpGet("csv")]
        [Consumes("application/json")]
        [Produces("text/csv")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult ExportToCsv([FromQuery] ExportBankTransactionsToCsvRequest request)
        {
            var filters = this.Mapper.Map<BankTransactionFilters>(request);

            var csvString = this.BankTransactionService.ExportToCsv(filters);

            var csv = Encoding.ASCII.GetBytes(csvString);

            this.Response.Headers.Add("Content-Disposition", $"inline; filename=\"BankTransactionReport.csv\"");
            return new FileContentResult(csv, "text/csv");
        }
    }
}