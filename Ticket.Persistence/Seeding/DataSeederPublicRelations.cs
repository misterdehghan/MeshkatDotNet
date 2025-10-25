using Azmoon.Domain.Entities.PublicRelations.Main;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Persistence.Seeding
{
    public class DataSeederPublicRelations
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {

            foreach (var Messengeritem in GetMessenger())
            {
                modelBuilder.Entity<Messenger>().HasData(Messengeritem);
            }

            foreach (var Operatoritem in GetOperator())
            {
                modelBuilder.Entity<Operator>().HasData(Operatoritem);
            }

        }
        private static IEnumerable<Messenger> GetMessenger()
        {
            return new List<Messenger>()
            {
                 new Messenger() { Id = 1, IsRemoved=false, PersianName = "واتس آپ", LatinName="WhatsApp"},
                 new Messenger() { Id = 2, IsRemoved=false, PersianName = "تلگرام", LatinName="Telegram"},
                 new Messenger() { Id = 3, IsRemoved=false, PersianName = "اینستاگرام", LatinName="Instagram"},
                 new Messenger() { Id = 4, IsRemoved=false, PersianName = "ایکس", LatinName="X"},
                 new Messenger() { Id = 5, IsRemoved=false, PersianName = "سروش پلاس", LatinName="Soroush+"},
                 new Messenger() { Id = 6, IsRemoved=false, PersianName = "آی گپ", LatinName="IGap"},
                 new Messenger() { Id = 7, IsRemoved=false, PersianName = "ایتا", LatinName="Eitaa"},
                 new Messenger() { Id = 8, IsRemoved=false, PersianName = "بله", LatinName="Bale"},
                 new Messenger() { Id = 9, IsRemoved=false, PersianName = "روبیکا", LatinName="Rubika"},
            };

        }

        private static IEnumerable<Operator> GetOperator()
        {
            return new List<Operator>()
            {
                new Operator()  { Id = Guid.NewGuid(), Name = "آذربایجان شرقی", NormalizedName="EastAzarbaijan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "آذربایجان غربی" ,  NormalizedName="WesternAzerbaijan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "اردبیل" ,  NormalizedName="Ardabil", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "اصفهان" ,  NormalizedName="Esfahan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "البرز" ,  NormalizedName="Alborz", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "ایلام" ,  NormalizedName="Ilam", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "بوشهر" ,  NormalizedName="Bushehr", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "تهران" ,  NormalizedName="Tehran", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "چهارمحال و بختیاری" ,  NormalizedName="ChaharmahalandBakhtiari", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "خراسان جنوبی" ,  NormalizedName="southernKhorasan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "خراسان رضوی" ,  NormalizedName="KhorasanRazavi", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "خراسان شمالی" ,  NormalizedName="NorthKhorasan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "خوزستان" ,  NormalizedName="Khuzestan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "زنجان" ,  NormalizedName="Zanjan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "سمنان" ,  NormalizedName="Semnan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "سیستان و بلوچستان" ,  NormalizedName="SistanandBaluchestan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "فارس" ,  NormalizedName="Fars", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "قزوین" ,  NormalizedName="Qazvin", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "قم" ,  NormalizedName="Qom", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "کردستان" ,  NormalizedName="Kurdistan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "کرمان" ,  NormalizedName="Kerman", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "کرمانشاه" ,  NormalizedName="Kermanshah", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "کهگیلویه و بویراحمد" ,  NormalizedName="KohgiloyehandBoyerahmad", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "گلستان" ,  NormalizedName="Golestan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "گیلان" ,  NormalizedName="Guilan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "لرستان" ,  NormalizedName="Lorestan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "مازندران" ,  NormalizedName="Mazandaran", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "مرکزی" ,  NormalizedName="Central", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "هرمزگان" ,  NormalizedName="Hormozgan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "همدان" ,  NormalizedName="Hamedan", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "یزد" ,  NormalizedName="Yazd", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "ارشد" ,  NormalizedName="Senior", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "بنیاد تعاون" ,  NormalizedName="Bonyad", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "دانشگاه" ,  NormalizedName="Daneshgah", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "راهور" ,  NormalizedName="Rahvar", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "ساحفا" ,  NormalizedName="Sahefa", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "ستاد فراجا" ,  NormalizedName="SetadFaraja", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "مرزبانی" ,  NormalizedName="Marzbani", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "مواد مخدر" ,  NormalizedName="MavadMokhader", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "نظام وظیفه" ,  NormalizedName="NezamVazife", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "پلیس اطلاعات" ,  NormalizedName="Etelaat", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "پلیس پیشگیری" ,  NormalizedName="Pishgiri", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "یگان ویژه" ,  NormalizedName="yeganVijeh", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش ولیعصر(عج)" ,  NormalizedName="MAValiasr", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش باهنر" ,  NormalizedName="MABahonar", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش بهشتی" ,  NormalizedName="MABeheshti", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش بیگلری" ,  NormalizedName="MABiglari", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش دستغیب" ,  NormalizedName="MADastghayb", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش چمران" ,  NormalizedName="MAChamran", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م آموزش مالک اشتر" ,  NormalizedName="MAMalekAshtar", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "م اموزش ادیبی" ,  NormalizedName="MAAdibi", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "مرکز ثامن الحجج(ع)" ,  NormalizedName="MSamen", InsertTime=DateTime.Now, IsRemoved=false},
                new Operator()  { Id = Guid.NewGuid(), Name = "دانشکده عقیدتی سیاسی" ,  NormalizedName="DaneshkadehAghigati", InsertTime=DateTime.Now, IsRemoved=false}

            };

        }
    }
}
