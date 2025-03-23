using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Readify.Extensions
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success!;
        }
    }
}
