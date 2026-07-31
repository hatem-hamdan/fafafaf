using MyStore.DataAccess.DTOs;

namespace         MyStore.API
{
    public interface IAdTrackingService
    {
        Task TrackPurchaseAsync(string email, OrderCreateDto dto);
    }
}
