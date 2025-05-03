
namespace Renty.Server.Product.Domain.Repository
{
    public interface ICategoryRepository
    {
        Task<Categorys?> FindBy(CategoryType category);
    }
}