using System.ComponentModel.DataAnnotations;

namespace PIV_PROYECTO_Final.Models
{
    public class ErrorViewModel
    {
        [Required]
        public string? RequestId { get; set; }
        [Required]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}