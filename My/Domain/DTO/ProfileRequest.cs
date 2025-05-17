using Renty.Server.Attribute;
using Renty.Server.Product.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renty.Server.My.Domain.DTO
{
    public enum ImageAction
    {
        Upload,
        Delete,
        None
    }

    public class ProfileRequest
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? AccountNumber { get; set; }

        [AllowedExtensions(".jpeg", ".png", ".jpg", ".webp", ".gif")]
        public IFormFile? ProfileImage { get; set; }
        public required ImageAction ImageAction { get; set; }
    }
}