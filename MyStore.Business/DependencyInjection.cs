using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyStore.DataAccess.Models; // تأكد إن هذا اسم مجلد الـ Context عندك

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // هنا البزنس يسجل الـ DbContext حق الأكسس في الذاكرة
        services.AddDbContext<MyStoreContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}