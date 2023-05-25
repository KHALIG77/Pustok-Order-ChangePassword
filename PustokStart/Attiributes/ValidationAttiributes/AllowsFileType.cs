using System.ComponentModel.DataAnnotations;

namespace PustokStart.Attiributes.ValidationAttiributes
{
    public class AllowsFileType:ValidationAttribute
    {
        private readonly string[] _types;

        public AllowsFileType(params string[] types)
        {
            _types = types;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();
            if(value is IFormFile)
            {
                files.Add((IFormFile)value);    
            }
            else if (value is List<IFormFile>)
            {
                files= (List<IFormFile>)value;
            }
           foreach(IFormFile file in files) 
            {
                if (!_types.Contains(file.ContentType))
                {
                    return new ValidationResult("File type must be" + string.Join(",", _types));
                }
            }
              
               
           
            return ValidationResult.Success;
        }
    }
}
