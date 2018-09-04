using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LeadgenWebApi;

namespace LeadgenFrontend.Models
{
    public class EntryPageViewModel
    {
        public Campaign Campaign;
        public EntryViewModel Entry { get; set; }
    }

    public class EntryViewModel
    {
        public EntryViewModel()
        {
            Referral = new List<LeadGenReferralViewModel>();
        }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Cellphone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(200, MinimumLength = 2)]
        public string Surname { get; set; }

        public string ReferralId { get; set; }
        public string Slug { get; set; }
        public int CampaignId { get; set; }

        public string Source { get; set; }
        public string Medium { get; set; }
        public string ReportingCategory { get; set; }

        public List<LeadGenReferralViewModel> Referral { get; set; }

        [Display(Name = "Ts + Cs")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        public bool TaC { get; set; }
    }

    public class LeadGenReferralViewModel
    {
        [Display(Name = "First Name")]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [StringLength(200, MinimumLength = 2)]
        public string Surname { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}
