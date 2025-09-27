using MediatR;
using VehicleShowroomManagement.Application.GoodsReceipts.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for accepting goods receipts
    /// </summary>
    public class AcceptGoodsReceiptCommandHandler : IRequestHandler<AcceptGoodsReceiptCommand, bool>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AcceptGoodsReceiptCommandHandler(
            IRepository<GoodsReceipt> goodsReceiptRepository,
            IUnitOfWork unitOfWork)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AcceptGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _goodsReceiptRepository.GetByIdAsync(request.Id);
            if (goodsReceipt == null)
                return false;

            if (!goodsReceipt.CanBeAccepted())
                return false;

            goodsReceipt.AcceptReceipt();

            await _goodsReceiptRepository.UpdateAsync(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
