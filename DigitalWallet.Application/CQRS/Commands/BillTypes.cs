using DigitalWallet.Application.DTOs.BillTypes;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Entities;
namespace DigitalWallet.Application.CQRS.Commands.BillTypes
{
    public record CreateBillTypeCommand(CreateBillTypeDTO createBillTypeDTO) : IRequest<ReadBillTypeDTO>;
    public record UpdateBillTypeCommand(UpdateBillTypeDTO updateBillTypeDTO) : IRequest<ReadBillTypeDTO>;
    public record DeleteBillTypeCommand(Guid Id) : IRequest;
    public class CreateBillTypeCommandHandler(IBillTypeRepository billTypeRepository) : IRequestHandler<CreateBillTypeCommand, ReadBillTypeDTO>
    {
        public async Task<ReadBillTypeDTO> Handle(CreateBillTypeCommand request, CancellationToken cancellationToken)
        {
            var BillTypeEntity = new BillTypeEntity(Guid.Empty, request.createBillTypeDTO.BillerType, request.createBillTypeDTO.Name, request.createBillTypeDTO.Amount);
            var result = await billTypeRepository.CreateBillTypeAsync(BillTypeEntity);
            return new ReadBillTypeDTO(result.Id,result.BillerId,result.Name,result.Amount);
        }
    }
    public class UpdateBillTypeCommandHandler(IBillTypeRepository billTypeRepository) : IRequestHandler<UpdateBillTypeCommand, ReadBillTypeDTO>
    {
        public async Task<ReadBillTypeDTO> Handle(UpdateBillTypeCommand request, CancellationToken cancellationToken)
        {
            var BillTypeEntity = new BillTypeEntity(request.updateBillTypeDTO.Id, Guid.Empty, request.updateBillTypeDTO.Name, request.updateBillTypeDTO.Amount);
            var result = await billTypeRepository.UpdateBillTypeAsync(BillTypeEntity);
            return new ReadBillTypeDTO(result.Id,result.BillerId,result.Name,result.Amount);
        }
    }
    public class DeleteBillTypeCommandHandler(IBillTypeRepository billTypeRepository) : IRequestHandler<DeleteBillTypeCommand>
    {
        public async Task Handle(DeleteBillTypeCommand request, CancellationToken cancellationToken)
        {
            await billTypeRepository.DeleteBillTypeAsync(request.Id);
        }
    }
}
