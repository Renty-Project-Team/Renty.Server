using Microsoft.EntityFrameworkCore;
using Renty.Server.Product.Domain;
using Renty.Server.Product.Domain.Repository;

namespace Renty.Server.Product.Infrastructer
{
    public class CategoryRepository(RentyDbContext dbContext) : ICategoryRepository
    {
        public async Task<Categorys?> FindBy(CategoryType category)
        {
            return await dbContext.Categorys.FirstOrDefaultAsync(c => c.Name == category);
        }
    }
}
