using System.ComponentModel.DataAnnotations;

namespace Renty.Server.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(params string[] extensions)
        {
            // 콤마로 구분된 문자열을 소문자 배열로 변환 (앞뒤 공백 제거, '.' 포함)
            _extensions = [.. extensions.Select(ext => ext.Trim().ToLowerInvariant())];

            // 기본 에러 메시지 설정 (필요시 재정의 가능)
            ErrorMessage = $"파일은 다음과 같은 확장자만 가능합니다. : {string.Join(", ", _extensions)}";
        }

        private bool CheckExtension(IFormFile file)
        {
            // 파일 확장자 가져오기 (소문자로 변환)
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            // 확장자가 없거나 허용 목록에 없는 경우 실패 반환
            return !string.IsNullOrEmpty(fileExtension) && _extensions.Contains(fileExtension);
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // 값이 List<IFormFile> 또는 IEnumerable<IFormFile> 타입인지 확인
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    // 리스트 내의 개별 파일이 null인 경우 건너뛰거나 유효성 검사 실패 처리 (선택)
                    if (file == null)
                    {
                        continue; // 또는 return new ValidationResult("File list contains null entries.");
                    }
                    
                    if (!CheckExtension(file)) return new ValidationResult(ErrorMessage);
                }
            }
            else if (value is IFormFile file) // 단일 파일도 처리 가능하도록 추가 (선택적)
            {
                if (file != null)
                {
                    if (CheckExtension(file)) return new ValidationResult(ErrorMessage);
                }
            }

            // 모든 파일이 유효하면 성공 반환
            return ValidationResult.Success!;
        }
    }
}
