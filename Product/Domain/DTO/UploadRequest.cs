using Renty.Server.Attribute;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Product.Domain.DTO
{
    public class UploadRequest
    {
        [Required(ErrorMessage = "제목은 필수 항목 입니다.")]
        [StringLength(50, ErrorMessage = "제목은 50자 이내로 입력해주세요.")]
        public required string Title { get; set; }

        [EnumDataType(typeof(CategoryType), ErrorMessage = "유효하지 않는 카테고리 입니다.")]
        public required CategoryType Category { get; set; }

        [EnumDataType(typeof(PriceUnit), ErrorMessage = "가격 기간 단위가 잘못되었습니다.")]
        public required PriceUnit Unit { get; set; }

        [Required(ErrorMessage = "가격은 필수 항목 입니다.")]
        [Range(0, int.MaxValue, ErrorMessage = "가격은 0원 보다 작을 수 없습니다.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "내용은 필수 항목 입니다.")]
        [StringLength(2000, ErrorMessage = "내용은 2000자 이내로 입력해 주세요.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "보증금은 필수 항목 입니다.")]
        [Range(0, int.MaxValue, ErrorMessage = "보증금은 0원 보다 작을 수 없습니다.")]
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "사진은 필수입니다.")]
        [MinLength(1, ErrorMessage = "최소 1장 이상이여야 합니다.")]
        [MaxLength(10, ErrorMessage = "10장을 초과할 수 없습니다.")]
        [AllowedExtensions(".jpeg", ".png", ".jpg", ".webp", ".gif")]
        public required List<IFormFile> Images { get; set; }
    }
}
