using MyStore.DataAccess.DTOs;

namespace MyStore.Services
{
    public interface IAdTrackingService
    {
        Task TrackPurchaseAsync(string email, OrderCreateDto dto);
    }
}
