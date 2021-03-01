using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="You have to enter User name")]
        [Display(Name ="User Name")]
        [MinLength(3,ErrorMessage ="Min 3 characters"),MaxLength(10,ErrorMessage ="Max 10 characters")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }


        [Required(ErrorMessage ="You have to enter your Email")]
        [DataType(DataType.EmailAddress)]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Required(ErrorMessage = "You have to enter your First Name")]
        [MinLength(3, ErrorMessage = "Min 3 characters"),MaxLength(15, ErrorMessage = "Max 15 characters")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You have to enter your Last Name")]
        [MinLength(3, ErrorMessage = "Min 3 characters"), MaxLength(15, ErrorMessage = "Max 15 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "You have to enter a password")]
        //[DataType(DataType.Password,ErrorMessage ="Min 8 characters")]
        //[MinLength(8),MaxLength(12)]
        ////[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",ErrorMessage = "Minimum 8 characters, at least one letter, one number and one special character")]
        //public string Password { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
        
        
        [Required]
        public Gender Gender { get; set; }
        
        
        [Required(ErrorMessage ="Specify your Birth date")]
        [DataType(DataType.Date)]
        [Display(Name ="Birth Date")]
        public DateTime BirthDate { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
