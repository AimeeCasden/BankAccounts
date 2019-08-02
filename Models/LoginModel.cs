using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class LoginUser
    {
       [Key]

       [Required]
       [EmailAddress]
       public string Email {get;set;}

       [Required]
       [DataType(DataType.Password)]
       public string Password {get;set;}

       

    }
}