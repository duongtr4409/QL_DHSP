using System.ComponentModel.DataAnnotations;
using Ums.Core.Domain.Connect;

namespace Ums.Models.Connect
{
    public class FeedbackModel : Feedback
    {
        [Required]
        public new string Title { get; set; }
    }
}