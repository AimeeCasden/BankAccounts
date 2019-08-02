using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class User
    {
       [Key]
       public int UserId {get;set;}

       [Required]
       [MinLength(2)]
       public string FirstName {get;set;}

       [Required]
       [MinLength(2)]
       public string LastName {get;set;}

       [Required]
       [EmailAddress]
       public string Email {get;set;}

       [Required]
       [DataType(DataType.Password)]
       [MinLength(5, ErrorMessage = "Password must be at least 5 characters or longer")]
       public string Password {get;set;}

       [NotMapped]
       [Compare("Password")]
       [DataType(DataType.Password)]
       public string Confirm{get;set;}
       public List<Transaction> UserTrans {get;set;}
    }

    
    public class Transaction
    {
        public int TransactionID{get;set;}

        public Double Amount {get;set;} = 0;
        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public int UserId{get;set;}
        public User Money {get;set;}

        
    }
}