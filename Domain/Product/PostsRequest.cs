using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Domain.Product
{
    public class PostsRequest
    {
        public List<CategoryType> Categorys { get; set; } = [];

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreatedAt { get; set; }

        public List<string> TitleWords { get; set; } = [];
    }
}
