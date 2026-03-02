using DigitalWallet.Application.CQRS.Queries.Transactions;
using DigitalWallet.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DigitalWallet.API.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController(IMediator mediator) : ControllerBase
    {
        /*READ ALL Transactions*/
        /// <summary>
        /// Retrieves a collection of all available Transactions.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a list of Transactions if any exist; otherwise, a response indicating
        /// that no Transactions were found.</returns>
        /// <response code="200">Transactions were retrieved successfully.</response>
        /// <response code="404">No Transactions were found.</response>
        [HttpGet(Name = "ReadAllTransactions")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transactions were retrieved successfully.", typeof(TransactionEntity))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No Transactions were found.")]
        public async Task<IActionResult> ReadAllTransactions()
        {
            ReadAllTransactionsQuery query = new();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
