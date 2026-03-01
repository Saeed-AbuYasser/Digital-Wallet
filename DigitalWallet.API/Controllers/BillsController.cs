using DigitalWallet.Application.CQRS.Commands.Bills;
using DigitalWallet.Application.CQRS.Queries.Bills;
using DigitalWallet.Application.DTOs.Bills;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
namespace DigitalWallet.API.Controllers
{
    [Route("api/Bills")]
    [ApiController]
    public class BillsController(IMediator mediator) : ControllerBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum BillType
        {
            Electricity,
            Water,
            Internet,
            Mobile,
            Gas,
            Rent,
            Other
        }
        /*READ ALL BILLS*/
        /// <summary>
        /// Retrieves all bills from the system.
        /// </summary>
        /// <remarks>The response includes a list of <see cref="ReadBillDTO"/> objects when bills are
        /// found. If no bills are available, the method returns a 404 Not Found status. This endpoint is typically used
        /// to display or process all bills in the system.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bills if any exist; otherwise, a 404 Not Found
        /// result.</returns>
        /// <response code="200">Bills were retrieved successfully.</response>
        /// <response code="404">No bills were found.</response>
        [HttpGet("{type:alpha}", Name = "GetAllBills")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Bills were retrieved successfully", Type = typeof(IEnumerable<ReadBillDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No bills were found.")]
        public async Task<IActionResult> GetAllBills(BillType type)
        {
            var query = new ReadAllBillsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /*READ PAID BILLS FOR SPECIFIC WALLET*/
        /// <summary>
        /// Retrieves all paid bills for a specific wallet from the system.
        /// </summary>
        /// <remarks>The response includes a list of <see cref="ReadBillDTO"/> objects when paid bills are
        /// found. If no paid bills are available, the method returns a 404 Not Found status. This endpoint is typically used
        /// to display or process all unpaid bills belongs to the provided wallet in the system.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of paid bills that belongs to a specific wallet if any exist; otherwise, a 404 Not Found
        /// result.</returns>
        /// <response code="200">Paid bills were retrieved successfully.</response>
        /// <response code="404">No paid bills were found.</response>
        [HttpGet("Paid/{WalletId}", Name = "GetPaidBillsByWalletId")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Paid bills were retrieved successfully", Type = typeof(IEnumerable<ReadBillDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No paid bills were found for this wallet.")]
        public async Task<IActionResult> GetPaidBillsByWalletId(Guid WalletId)
        {
            var query = new ReadPaidBillsByWalletIdQuery(WalletId);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /*READ UNPAID BILLS FOR SPECIFIC WALLET*/
        /// <summary>
        /// Retrieves all unpaid bills for a specific wallet from the system.
        /// </summary>
        /// <remarks>The response includes a list of <see cref="ReadBillDTO"/> objects when unpaid bills are
        /// found. If no unpaid bills are available, the method returns a 404 Not Found status. This endpoint is typically used
        /// to display or process all unpaid bills belongs to the provided wallet in the system.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of unpaid bills that belongs to a specific wallet if any exist; otherwise, a 404 Not Found
        /// result.</returns>
        /// <response code="200">UnPaid bills were retrieved successfully.</response>
        /// <response code="404">No unpaid bills were found.</response>
        [HttpGet("UnPaid/{WalletId}", Name = "GetUnPaidBillsByWalletId")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Unpaid bills were retrieved successfully", Type = typeof(IEnumerable<ReadBillDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, $"No unpaid bills were found for this wallet.")]
        public async Task<IActionResult> GetUnPaidBillsByWalletId(Guid WalletId)
        {
            var query = new ReadUnPaidBillsByWalletIdQuery(WalletId);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /*READ BILL BY ID*/
        /// <summary>
        /// Retrieves the bill that matches the specified unique identifier.
        /// </summary>
        /// <param name="BillId">The unique identifier of the bill to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the bill data if found; otherwise, a 404 Not Found result if no
        /// bill with the specified identifier exists.</returns>
        /// <response code="200">The bill was retrieved successfully.</response>
        /// <response code="404">No bill was found with the specified identifier.</response>
        [HttpGet("{BillId:guid}", Name = "GetBillById")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Bill was retrieved successfully", Type = typeof(IEnumerable<ReadBillDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "This ID doesn't belong to any bill.")]
        public async Task<IActionResult> GetBillById(Guid BillId)
        {
            var query = new ReadBillByIdQuery(BillId);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /*ISSUE NEW BILL*/
        /// <summary>
        /// Issues a new bill for the specified wallet and bill type.
        /// </summary>
        /// <remarks>This endpoint creates a new bill associated with the given wallet and bill type. The
        /// wallet and bill type must exist; otherwise, a 404 response is returned. The response includes the details of
        /// the issued bill upon success.</remarks>
        /// <param name="WalletId">The unique identifier of the wallet for which the bill will be issued.</param>
        /// <param name="BillTypeId">The unique identifier of the bill type to be issued for the wallet.</param>
        /// <returns>An <see cref="IActionResult"/> containing a <see cref="ReadBillDTO"/> if the bill is issued successfully;
        /// otherwise, a 404 Not Found response if the wallet or bill type does not exist.</returns>
        /// <response code="201">The bill was issued successfully.</response>
        /// <response code="404">The specified wallet or bill type does not exist.</response>
        [HttpPost("Issue/{WalletId}/{BillTypeId}", Name = "IssueBill")]
        [SwaggerResponse(StatusCodes.Status201Created, description: "Bill was issued successfully", Type = typeof(ReadBillDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "This wallet or bill type doesn't exist.")]
        public async Task<IActionResult> IssueBill(Guid WalletId, Guid BillTypeId)
        {
            var command = new IssueBillCommand(new CreateBillDTO(WalletId, BillTypeId));
            var result = await mediator.Send(command);
            return CreatedAtAction("IssueBill",result);
        }

        /*PAY BILL*/
        /// <summary>
        /// Pay the bill associated with the given ID.
        /// </summary>
        /// <remarks>This endpoint paid the bill associated with the given ID. The
        /// bill ID must exist; otherwise, a 404 response is returned.</remarks>
        /// <param name="BillId">The unique identifier of the bill to be paid.</param>
        /// <response code="200">The bill was paid successfully.</response>
        /// <response code="404">The specified bill does not exist.</response>
        [HttpPatch("Pay/{BillId}", Name = "PayBill")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Bill was paid successfully", Type = typeof(ReadBillDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "This bill doesn't exist.")]
        public async Task<IActionResult> PayBill(Guid BillId)
        {
            var command = new PayBillCommand(BillId);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
