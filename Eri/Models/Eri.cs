using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        public int Spend_Id { get; set; }
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
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public bool Del_Flag { get; set; }
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
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public bool Del_Flag { get; set; }
    }

    public class Mst_ConnectionInfo
    {
        [Key]
        public int Id { get; set; }
        public string Ip_Address { get; set; }
        public int Port { get; set; }
    }

    public class SeedData
    { 
        public void InsertSeedData()
        {
            using (var db = new MyContext())
            {
                db.Database.EnsureCreated();
                int cnt_income = db.Mst_Income.Count();
                if (cnt_income == 0)
                {
                    ExecSeedIncome(db);
                }
                int cnt_spend = db.Mst_Spend.Count();
                if (cnt_spend == 0)
                {
                    ExecSeedSpend(db);
                }
                return;
            }
        }

        private async void ExecSeedIncome(MyContext db)
        {
            db.Database.EnsureCreated();
            await db.Mst_Income.AddRangeAsync(
                new Mst_Income { Income_Name = "給料", Icon_Name = "salary.png" },
                new Mst_Income { Income_Name = "おこずかい", Icon_Name = "kozukai.png" },
                new Mst_Income { Income_Name = "ボーナス", Icon_Name = "bonus.png" }
            );
            await db.SaveChangesAsync();
        }

        private async void ExecSeedSpend(MyContext db)
        {
            db.Database.EnsureCreated();
            await db.Mst_Spend.AddRangeAsync(
                new Mst_Spend { Spend_Name = "食費", Icon_Name = "eat_home.png" },
                new Mst_Spend { Spend_Name = "外食費", Icon_Name = "gaishoku.png" },
                new Mst_Spend { Spend_Name = "ガソリン", Icon_Name = "gus.png" },
                new Mst_Spend { Spend_Name = "日用品", Icon_Name = "nichiyou.png" },
                new Mst_Spend { Spend_Name = "子供関係", Icon_Name = "baby.png" },
                new Mst_Spend { Spend_Name = "水光熱費", Icon_Name = "suikounetsu.png" },
                new Mst_Spend { Spend_Name = "ファッション", Icon_Name = "fation.png" },
                new Mst_Spend { Spend_Name = "病院", Icon_Name = "hospital.png" },
                new Mst_Spend { Spend_Name = "携帯電話", Icon_Name = "phon.png" },
                new Mst_Spend { Spend_Name = "交際費", Icon_Name = "asobi.png" },
                new Mst_Spend { Spend_Name = "ヘアー", Icon_Name = "hair.png" },
                new Mst_Spend { Spend_Name = "車維持費／整備費", Icon_Name = "car.png" },
                new Mst_Spend { Spend_Name = "旅行", Icon_Name = "train.png" },
                new Mst_Spend { Spend_Name = "家電購入費", Icon_Name = "kaden.png" },
                new Mst_Spend { Spend_Name = "プレゼント", Icon_Name = "present.png" }
            );
            await db.SaveChangesAsync();
        }

    }
}