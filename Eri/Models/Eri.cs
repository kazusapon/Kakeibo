using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eri.Models
{
    public class Mst_Spend
    {
        [Key]
        public int Id { get; set; }
        public string Spend_Name { get; set; }
        public string Icon_Name { get; set; }
    }

    public class Mst_Income
    {
        [Key]
        public int Id { get; set; }
        public string Income_Name { get; set; }
        public string Icon_Name { get; set; }
    }

    public class Mst_Fixed
    {
        [Key]
        public int Id { get; set; }
        public string Fixed_Name { get; set; }
        public int Money { get; set; }
        public bool Del_Flag { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }

    public class Mst_Available
    {
        [Key]
        public int Id { get; set; }
        public int Money { get; set; }
        public bool Del_Flag { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }

    public class Mst_User
    {
        [Key]
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Login_Id { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
    }

    public class Tra_Spending
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Spend_Id { get; set; }
        public int Money { get; set; }
        public DateTime Purchase_Date { get; set; }
        public string Description { get; set; }
        public bool Linked_Flag { get; set; }
    }

    public class Tra_Income
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Income_Id { get; set; }
        public int Money { get; set; }
        public DateTime Payment_Date { get; set; }
        public string Description { get; set; }
        public bool Linked_Flag { get; set; }
    }
}