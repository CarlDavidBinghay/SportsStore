using Microsoft.EntityFrameworkCore;
using SportsStore.Data;

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private SportsStoreDbContext context;

        public EFStoreRepository(SportsStoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
    }
}