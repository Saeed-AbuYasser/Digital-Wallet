using DigitalWallet.Application.CQRS.Commands.Billers;
using DigitalWallet.Application.CQRS.Queries.Billers;
using DigitalWallet.Application.DTOs.Billers;
using DigitalWallet.Application.DTOs.Wallets;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DigitalWallet.API.Controllers
{
    [Route("api/Billers")]
    [ApiController]
    public class BillersController(IMediator mediator) : ControllerBase
    {

        /*CREATE BILL*/
        /// <summary>
        /// Creates a new biller with the specified identifier and name.
        /// </summary>
        /// <param name="WalletId">The unique identifier for the biller to be created.</param>
        /// <param name="Name">The name of the biller. Cannot be null or empty.</param>
        /// <returns>A result that indicates the outcome of the operation. Returns a 201 Created response with the created biller
        /// if successful; otherwise, returns an error response.</returns>
        /// <response code="201">The biller was created successfully.</response>
        /// <response code="400">Validation failed. The request data is invalid.</response>
        /// <response code="409">A biller with the same name already exists. The request cannot be processed due to a conflict.</response>
        [HttpPost("create/{WalletId}/{Name}", Name = "CreateBiller")]
        [SwaggerOperation(Summary = "Creates a biller", Description = "Longer description here")]
        [SwaggerResponse(201, "The biller was created successfully.", typeof(ReadBillerDTO))]
        [SwaggerResponse(400, "Validation failed.")]
        [SwaggerResponse(409, "A biller with the same name already exists.")]
        public async Task<IActionResult> Create(Guid WalletId, string Name)
        {
            CreateBillerCommand command = new(new CreateBillerDTO(WalletId, Name));
            var result = await mediator.Send(command);
            return CreatedAtAction("Create", result);
        }
        /*READ ALL BILLERS*/
        /// <summary>
        /// Retrieves a collection of all available billers.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a list of billers if any exist; otherwise, a response indicating
        /// that no billers were found.</returns>
        /// <response code="200">Billers were retrieved successfully.</response>
        /// <response code="404">No billers were found.</response>
        [HttpGet(Name = "ReadAllBillers")]
        [SwaggerResponse(StatusCodes.Status200OK, "Billers were retrieved successfully.", typeof(ReadBillerDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No Billers were found.")]
        public async Task<IActionResult> ReadAllBillers()
        {
            ReadAllBillersQuery query = new();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
