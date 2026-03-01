using DigitalWallet.Application.DTOs.Billers;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.CQRS.Commands.Billers
{
    public record CreateBillerCommand(CreateBillerDTO createBillerDTO):IRequest<ReadBillerDTO>;
    public class CreateBillerCommandHandler(IBillerRepository billerRepository) : IRequestHandler<CreateBillerCommand, ReadBillerDTO>
    {
        public async Task<ReadBillerDTO> Handle(CreateBillerCommand request, CancellationToken cancellationToken)
        {
            BillerEntity billerEntity = new(Guid.Empty,request.createBillerDTO.WalletId,request.createBillerDTO.Name); 
            var result = await billerRepository.CreateBillerAsync(billerEntity);
            return new(result.Id, result.WalletId, result.Name);
        }
    }
}
