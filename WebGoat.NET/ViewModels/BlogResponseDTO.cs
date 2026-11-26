using System.ComponentModel.DataAnnotations;
using WebGoatCore.Models;

namespace WebGoatCore.ViewModels
{
    // Step 2: DTO'en beskriver kun det felt, brugeren må sende.
    // Forfatter(skal sættes af applikation), 
    // dato(Skal sættes af server) 
    // og id er udeladt, så de ikke kan ændres fra input (eller måske i burp).
    public class BlogResponseDTO
    {
        // Validering afviser tomme svar og begrænser længden.
        [Required(ErrorMessage = "Indtast venligst respons.")]
        [StringLength(BlogResponseContent.MaxLength, ErrorMessage = "Respons kan ikke overstige {1} karakterer.")]
        public string? Contents { get; set; }
    }
}
