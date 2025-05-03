using Renty.Server.Global;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Product.Domain.DTO
{
    public class PostsRequest
    {
        public List<CategoryType> Categorys { get; set; } = [];

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreatedAt { get; set; }

        public List<string> TitleWords { get; set; } = [];
    }
}
