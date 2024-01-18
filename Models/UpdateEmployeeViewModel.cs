﻿using System.ComponentModel.DataAnnotations;

namespace IDM.Models
{
   
    public class UpdateEmployeeViewModel
    {
         public Guid Id { get; set; }
        // public string? FirstName { get; set; }
        // public string? LastName { get; set; }
        // public string? email { get; set; }
        // public string? userPassword { get; set; }
        // public long Salary { get; set; }
        // public DateTime DateOfBirth { get; set; }
        // public string? Department { get; set; }
        // public string? AdhaarNumber { get; set; }
        // public string? Address { get; set; }
        // public string? MobileNumber { get; set; }
        // public string? Gender { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must contain Minimum 3 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
        public string? FirstName { get; set; }

       [Required(ErrorMessage = "Last name is required")]
       [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
       [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Last name should only contain letters and spaces")]
        public string? LastName { get; set; }

        [Required(ErrorMessage ="Email is required")]  
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(50)] 
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z]+\.[a-z]{2,4}", ErrorMessage = "Enter Correct Email ID")] 
        public string? email { get; set; }

        [Required(ErrorMessage ="Password is required")]  
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Minimum of 3 to 15 characters", MinimumLength = 3)]
        public string? userPassword { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public long Salary { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }

        [StringLength(100, ErrorMessage = "Enter Valid Adhaar Number", MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Adhaar number must be 12 digits long")]
        public string? AdhaarNumber { get; set; }

        [Required(ErrorMessage =" Address is required")]  
        public string? Address { get; set; }

        [Required(ErrorMessage = "Contact is required")]  
        [DataType(DataType.PhoneNumber)]  
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

    }
}
