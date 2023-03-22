using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ums.Models.Personnel
{
    public class StaffModel
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int TrialYear { get; set; }
        public int TrialMonth { get; set; }
        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }
        [Range(1, int.MaxValue)]
        public int TeachingInId { get; set; }
        [Required]
        public string Gender { get; set; }
        public int DegreeId { get; set; }
        public int TitleId { get; set; }
        public int Level { get; set; }
        public bool IsProbation { get; set; }
        public string AccountNumber { get; set; }
        public string TaxNumber { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Degrees { get; set; }
        public IEnumerable<SelectListItem> Titles { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }
        public int FrameExceeded { get; set; }
        public string SalaryRaiseOn { get; set; }
        public bool IsRetired { get; set; }
        public bool IsMoved { get; set; }
        public string RetireOrMoveDate { get; set; }
        public string MovedTo { get; set; }
        [Required]
        public string Birthday { get; set; }
    }
}