using DigitalWallet.Application.CQRS.Commands.BillTypes;
using DigitalWallet.Application.CQRS.Queries.BillTypes;
using DigitalWallet.Application.DTOs.BillTypes;
using DigitalWallet.Application.DTOs.Wallets;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DigitalWallet.API.Controllers
{
    [Route("api/BillTypes")]
    [ApiController]
    public class BillTypesController(IMediator mediator) : ControllerBase
    {
        /*READ ALL BILL TYPES*/
        /// <summary>
        /// Retrieves all available bill types.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bill types if found; otherwise, a 404 Not Found
        /// result if no bill types exist.</returns>
        /// <response code="200">Bill types were retrieved successfully.</response>
        /// <response code="404">No bill types were found.</response>
        [HttpGet(Name = "ReadAllBillTypes")]
        [SwaggerResponse(StatusCodes.Status200OK, "Bill types were retrieved successfully.", typeof(ReadWalletDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No bill types were found.")]
        public async Task<IActionResult> ReadAllBillTypes()
        {
            var query = new ReadAllBillTypesQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /*CREATE BILL TYPE*/
        /// <summary>
        /// Creates a new bill type for the specified biller with the given name and amount.
        /// </summary>
        /// <remarks>Returns a 400 Bad Request response if validation fails, or a 409 Conflict response if
        /// a bill type with the same name already exists for the specified biller.</remarks>
        /// <param name="BillerId">The unique identifier of the biller for whom the bill type is being created.</param>
        /// <param name="Name">The name of the bill type to create. Must consist of alphabetic characters only.</param>
        /// <param name="Amount">The amount associated with the bill type. Must be a valid decimal value.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns a 201 Created response
        /// with details of the created bill type if successful; otherwise, returns an error response.</returns>
        /// <response code="201">The bill type was created successfully.</response>
        /// <response code="400">Invalid data somewhere.</response>
        /// <response code="409">A bill type with the same name already exists.</response>
        [HttpPost("create/{BillerId:guid}/{Name:alpha}/{Amount:decimal}", Name = "CreateBillType")]
        [SwaggerOperation(Summary = "Creates a bill type", Description = "Longer description here")]
        [SwaggerResponse(201, "The bill type was created successfully.", typeof(ReadBillTypeDTO))]
        [SwaggerResponse(400, "Invalid data somewhere.")]
        [SwaggerResponse(409, "A bill type with the same name already exists.")]
        public async Task<IActionResult> Create(Guid BillerId, string Name, decimal Amount)
        {
            var command = new CreateBillTypeCommand(new(BillerId, Name, Amount));
            var result = await mediator.Send(command);
            return CreatedAtAction("Create", result);
        }
        /*READ BILL TYPES BY BILLER ID*/
        /// <summary>
        /// Retrieves the collection of bill types associated with the specified biller identifier.
        /// </summary>
        /// <param name="BillerId">The unique identifier of the biller whose bill types are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bill types if found; otherwise, a 404 Not Found
        /// response if no bill types exist for the specified biller.</returns>
        /// <response code="200">Bill types were retrieved successfully.</response>
        /// <response code="404">No bill types were found for this biller.</response>
        [HttpGet("{BillerId:guid}", Name = "ReadBillTypeByBillerId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Bill types were retrieved successfully.", typeof(IEnumerable<ReadWalletDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No bill types were found for this biller.")]
        public async Task<IActionResult> ReadBillByBillerIdTypes(Guid BillerId)
        {
            var query = new ReadBillTypesByBillerIdQuery(BillerId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
