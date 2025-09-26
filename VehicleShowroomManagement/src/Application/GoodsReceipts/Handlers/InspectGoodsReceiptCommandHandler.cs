using MediatR;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for inspecting goods receipts
    /// </summary>
    public class InspectGoodsReceiptCommandHandler : IRequestHandler<InspectGoodsReceiptCommand, bool>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InspectGoodsReceiptCommandHandler(
            IRepository<GoodsReceipt> goodsReceiptRepository,
            IUnitOfWork unitOfWork)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(InspectGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _goodsReceiptRepository.GetByIdAsync(request.Id);
            if (goodsReceipt == null)
                return false;

            if (!goodsReceipt.CanBeInspected())
                return false;

            goodsReceipt.MarkAsInspected(request.InspectedBy, request.InspectionNotes);

            await _goodsReceiptRepository.UpdateAsync(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
