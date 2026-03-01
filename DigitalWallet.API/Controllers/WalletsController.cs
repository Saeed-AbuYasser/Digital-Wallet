using DigitalWallet.Application.CQRS.Commands.Wallets;
using DigitalWallet.Application.CQRS.Queries.Wallets;
using DigitalWallet.Application.DTOs.Wallets;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace DigitalWallet.API.Controllers
{
    [Route("api/Wallets")]
    [ApiController]
    public class WalletsController(IMediator mediator) : ControllerBase
    {
        /* READ ALL WALLETS*/
        /// <summary>
        /// Retrieves all wallets.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> that contains a collection of wallets if any exist; otherwise, a 404 Not
        /// Found result if no wallets are found.</returns>
        /// <remarks>
        /// Sample response:
        /// 
        ///     Wallet
        ///     {
        ///        "Id": 3fa85f64-5717-4562-b3fc-2c963f66afa6,
        ///        "Name": "Wallet #1",
        ///        "Balance": $5000.00
        ///     }
        /// </remarks>
        /// <response code="200">Returns a list of wallets.</response>
        /// <response code="404">If no wallets are found.</response>
        [HttpGet(Name = "GetAllWallets")]
        [SwaggerResponse(StatusCodes.Status200OK, "Wallets were retrieved successfully.", typeof(ReadWalletDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No wallets were found.")]
        public async Task<IActionResult> Get()
        {
            var query = new ReadAllWalletsQuery();
            var wallet = await mediator.Send(query);

            if (wallet == null || wallet.Count() < 1)
                return NotFound(new { message = $"No wallets were found." });

            return Ok(wallet);

        }
        /*READ ONE WALLET BY ID*/
        /// <summary>
        /// Retrieves the wallet with the specified unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the wallet to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the wallet data if found; otherwise, a 404 Not Found result.</returns>
        /// <remarks>
        /// Sample response:
        /// 
        ///     Wallet
        ///     {
        ///        "Id": 3fa85f64-5717-4562-b3fc-2c963f66afa6,
        ///        "Name": "Wallet #1",
        ///        "Balance": $5000.00
        ///     }
        /// </remarks>
        /// <response code="200">Returns the wallet with the specified ID.</response>
        /// <response code="404">If a wallet with the specified ID is not found.</response>
        [HttpGet("{Id:guid}", Name = "GetWalletById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Wallet was found successfully.",typeof(ReadWalletDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Wallet was not found.")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var query = new ReadWalletByIdQuery(Id);
            var wallet = await mediator.Send(query);

            if (wallet == null)
                return NotFound(new { message = $"Wallet with ID {Id} not found." });

            return Ok(wallet);
        }
        /*READ ONE WALLET BY HOLDER NAME*/
        /// <summary>
        /// Retrieves the wallet with the specified holder name.
        /// </summary>
        /// <param name="Name">The Name of the holder of the wallet to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the wallet data if found; otherwise, a 404 Not Found result.</returns>
        /// <remarks>
        /// Sample response:
        /// 
        ///     Wallet
        ///     {
        ///        "Id": 3fa85f64-5717-4562-b3fc-2c963f66afa6,
        ///        "Name": "Wallet #1",
        ///        "Balance": $5000.00
        ///     }
        /// </remarks>
        /// <response code="200">Returns the wallet with the specified Name.</response>
        /// <response code="404">If a wallet with the specified Name is not found.</response>
        [HttpGet("{Name:alpha}", Name = "GetWalletByName")]
        [SwaggerResponse(StatusCodes.Status200OK, "Wallet was found successfully.", typeof(ReadWalletDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Wallet was not found.")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var query = new ReadWalletByNameQuery(Name);
            var wallet = await mediator.Send(query);

            if (wallet == null)
                return NotFound(new { message = $"Wallet with ID {Name} not found." });

            return Ok(wallet);
        }
        /*CREATE ONE WALLET*/
        /// <summary>
        /// Creates a Digital Wallet.
        /// </summary>
        /// <param name="HolderName">The name of the person who wants to own the wallet</param>
        /// <param name="InitialBalance">The initial balance of the wallet. Must be a positive value.</param>
        /// <returns>A newly created Digital Wallet</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Wallet
        ///     {
        ///        "Name": "Wallet #1",
        ///        "Balance": $5000.00
        ///     }
        ///
        /// Sample response:
        /// 
        ///     Wallet
        ///     {
        ///        "Id": 3fa85f64-5717-4562-b3fc-2c963f66afa6,
        ///        "Name": "Wallet #1",
        ///        "Balance": $5000.00
        ///     }
        /// </remarks>
        /// 
        /// <response code="201">Returns the newly created wallet</response>
        /// <response code="400">If the wallet is null or missing required information</response>
        /// <resposne code="409">If a wallet with the same name already exists</resposne>
        [HttpPost("create/{HolderName:alpha}/{InitialBalance:decimal}",Name = "CreateNewWallet")]
        [SwaggerOperation(Summary = "Creates a wallet", Description = "Longer description here")]
        [SwaggerResponse(201, "The wallet was created successfully.", typeof(ReadWalletDTO))]
        [SwaggerResponse(400, "Validation failed.")]
        [SwaggerResponse(409, "A wallet with the same name already exists.")]
        public async Task<IActionResult> Create(string HolderName, decimal InitialBalance) 
        {
            var command = new CreateWalletCommand(new CreateWalletDTO(HolderName, InitialBalance));
            var wallet = await mediator.Send(command);
            return CreatedAtAction("Create",wallet);
        }
        /*WITHDRAW MONEY FROM WALLET BY ID*/
        /// <summary>
        /// Withdraws a specified amount from the wallet identified by the given ID.
        /// </summary>
        /// <param name="Id">The unique identifier of the wallet from which the amount will be withdrawn.</param>
        /// <param name="Amount">The amount to withdraw from the wallet. Must be a positive value.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the withdraw operation. Returns status code 200 (OK)
        /// if the withdrawal is successful, 400 (Bad Request) if the operation fails, or 404 (Not Found) if the wallet
        /// does not exist.</returns>
        /// <response code="200">Withdraw operation was successfully done.</response>
        /// <response code="400">Withdraw operation failed.</response>
        /// <response code="404">Wallet was not found.</response>
        /// <response code="409">Withdraw operation failed due to insufficient funds.</response>
        [HttpPatch("withdraw/{Id:guid}/{Amount:decimal}",Name = "WithdrawFromWallet")]
        [SwaggerResponse(StatusCodes.Status200OK,"Withdraw operation was successfully done.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,"Withdraw operation failed.")]
        [SwaggerResponse(StatusCodes.Status404NotFound,"Wallet was not found.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Withdraw operation failed due to insufficient funds.")]
        [SwaggerOperation(Summary = "Withdraws an amount from a wallet", Description = "Longer description here")]
        public async Task<IActionResult> Withdraw(Guid Id, decimal Amount)
        {
            var command = new WithdrawCommand(Id, Amount);
            await mediator.Send(command);
            return Ok();
        }
        /*DEPOSIT MONEY TO WALLET BY ID*/
        /// <summary>
        /// Deposits a specified amount from the wallet identified by the given ID.
        /// </summary>
        /// <param name="Id">The unique identifier of the wallet from which the amount will be deposed.</param>
        /// <param name="Amount">The amount to deposit from the wallet. Must be a positive value.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the deposit operation. Returns status code 200 (OK)
        /// if the deposit is successful, 400 (Bad Request) if the operation fails, or 404 (Not Found) if the wallet
        /// does not exist.</returns>
        /// <response code="200">Deposit operation was successfully done.</response>
        /// <response code="400">Deposit operation failed.</response>
        /// <response code="404">Wallet was not found.</response>
        [HttpPatch("deposit/{Id:guid}/{Amount:decimal}", Name = "DepositToWallet")]
        [SwaggerResponse(StatusCodes.Status200OK, "Deposit operation was successfully done.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Deposit operation failed.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Wallet was not found.")]
        [SwaggerOperation(Summary = "Deposit an amount from a wallet", Description = "Longer description here")]
        public async Task<IActionResult> Deposit(Guid Id, decimal Amount)
        {
            var command = new DepositCommand(Id, Amount);
            await mediator.Send(command);
            return Ok();
        }
        /*TRANSFER MONEY FROM SOURCE WALLET TO DESTINATION WALLET BY IDS*/
        /// <summary>
        /// Transfer a specified amount from the source wallet to the destination wallet identified by the given IDs.
        /// </summary>
        /// <param name="SourceWalletId">The unique identifier of the source wallet from which the amount will be transferred.</param>
        /// <param name="DestinationWalletId">The unique identifier of the destination wallet to which the amount will be transferred.</param>
        /// <param name="Amount">The amount to transfer from the source wallet to the destination wallet. Must be a positive value.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the transfer operation. Returns status code 200 (OK) 
        /// if the transformation is successful, 400 (Bad Request) if the amount is less than $1, or 404 (Not Found) if the 
        /// source wallet or the destination wallet or both were not found, or 409 (Conflict) if the source wallet has insufficient balance.</returns> 
        /// <response code="200">Transfer operation was successfully done.</response>
        /// <response code="400">Transfer operation failed. Amount most be more than $1.</response>
        /// <response code="404">One or both wallets were not found.</response>
        /// <response code="409">Withdraw operation failed due to insufficient funds.</response>
        [HttpPatch("transfer/{SourceWalletId:guid}/{DestinationWalletId:guid}/{Amount:decimal}", Name = "TransferMoneyBetweenWallets")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transfer operation was successfully done.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Transfer operation failed.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "One or both wallets were not found.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Transfer operation failed due to insufficient funds.")]
        public async Task<IActionResult> TransferMoney(Guid SourceWalletId, Guid DestinationWalletId, decimal Amount)
        {
            var command = new TransferMoneyCommand(SourceWalletId, DestinationWalletId, Amount);
            await mediator.Send(command);
            return Ok();
        }
        /*DELETE WALLET BY ID*/
        /// <summary>
        /// Deletes the wallet with the specified unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the wallet to delete. Must not be empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns status code 200 (OK) if the
        /// wallet was successfully deleted; otherwise, 404 (Not Found) if the wallet does not exist.</returns>
        /// <response code="200">Wallet was successfully deleted.</response>
        /// <response code="404">Wallet was not found.</response>
        [HttpDelete("delete/{Id:guid}",Name = "DeleteWallet")]
        [SwaggerResponse(StatusCodes.Status200OK, "Wallet was successfully deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Wallet was not found.",typeof(Guid))]
        [SwaggerOperation(Summary = "Deletes a wallet", Description = "Longer description here")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Delete operation failed due to existing dependencies.")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var command = new DeleteWalletByIdCommand(Id);
            await mediator.Send(command);
            return Ok();
        }


    }

}
