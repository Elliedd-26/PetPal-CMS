using System.ComponentModel.DataAnnotations;

namespace PetPalCMS.DTOs
{
    // 创建宠物用的DTO（输入）
    public class CreatePetDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Species { get; set; }
        [Required]
        public string Breed { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
    }

    // 返回宠物信息的DTO（输出）
    public class PetDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime Birthdate { get; set; }
    }

    // 创建兽医的DTO
    public class CreateVetDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ClinicName { get; set; }
        [Required]
        public string ContactInfo { get; set; }
    }

    public class VetDto
    {
        public int VetId { get; set; }
        public string Name { get; set; }
        public string ClinicName { get; set; }
        public string ContactInfo { get; set; }
    }

    // 创建疫苗记录的DTO
    public class CreateVaccinationRecordDto
    {
        [Required]
        public int PetId { get; set; }
        [Required]
        public int VetId { get; set; }
        [Required]
        public string VaccineName { get; set; }
        [Required]
        public DateTime VaccinationDate { get; set; }
    }

    public class VaccinationRecordDto
    {
        public int VaccinationRecordId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string VaccineName { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string PetName { get; set; }
        public string VetName { get; set; }
    }

    // API响应包装器
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ApiResponse<T> SuccessResult(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> ErrorResult(string message, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}