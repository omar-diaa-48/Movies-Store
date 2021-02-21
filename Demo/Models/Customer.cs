using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(2),MaxLength(15)]
        public string FirstName { get; set; }


        [Required]
        [MinLength(2), MaxLength(15)]
        public string LastName { get; set; }


        [Required]
        //[Index(IsClustered = false, IsUnique = true)]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Compare("EmailAddress")]
        public string ConfirmEmailAdress { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [MaxLength(200)]
        public string Address { get; set; }


        
        public Gender Gender { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Required]
        [StringLength(11)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
