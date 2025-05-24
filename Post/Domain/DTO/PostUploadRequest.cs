using Renty.Server.Attribute;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Post.Domain.DTO
{
    public class PostUploadRequest
    {
        [Required(ErrorMessage = "제목은 필수 항목 입니다.")]
        [StringLength(50, ErrorMessage = "제목은 50자 이내로 입력해주세요.")]
        public required string Title { get; set; }

        [EnumDataType(typeof(CategoryType), ErrorMessage = "유효하지 않는 카테고리 입니다.")]
        public required CategoryType Category { get; set; }

        [Required(ErrorMessage = "내용은 필수 항목 입니다.")]
        [StringLength(2000, ErrorMessage = "내용은 2000자 이내로 입력해 주세요.")]
        public required string Description { get; set; }

        [MaxLength(10, ErrorMessage = "10장을 초과할 수 없습니다.")]
        [AllowedExtensions(".jpeg", ".png", ".jpg", ".webp", ".gif")]
        public List<IFormFile> Images { get; set; } = [];
    }
}
