using System.ComponentModel.DataAnnotations;

namespace PustokStart.Attiributes.ValidationAttiributes
{
    public class MaxFileSize:ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSize(int maxSize)
        {
           _maxSize = maxSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();  
            if(value is IFormFile)
            {
                 files.Add((IFormFile)value);   
            }else if(value is List<IFormFile>) 
            {
                files= (List<IFormFile>)value;
            }
            foreach (var file in files)
            {

                if (file.Length > _maxSize)
                {
                    return new ValidationResult("File size must be less or equal than" + _maxSize + "byte");
                }
            }
            return ValidationResult.Success;
        }
    }
}
