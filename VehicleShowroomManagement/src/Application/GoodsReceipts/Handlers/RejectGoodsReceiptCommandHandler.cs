using MediatR;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for rejecting goods receipts
    /// </summary>
    public class RejectGoodsReceiptCommandHandler : IRequestHandler<RejectGoodsReceiptCommand, bool>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectGoodsReceiptCommandHandler(
            IRepository<GoodsReceipt> goodsReceiptRepository,
            IUnitOfWork unitOfWork)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RejectGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _goodsReceiptRepository.GetByIdAsync(request.Id);
            if (goodsReceipt == null)
                return false;

            if (!goodsReceipt.CanBeRejected())
                return false;

            goodsReceipt.RejectReceipt(request.Reason);

            await _goodsReceiptRepository.UpdateAsync(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
