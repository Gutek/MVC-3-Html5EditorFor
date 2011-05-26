using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Html5.Web.Models
{
    public class NumericViewModel
    {
        public byte Byte { get; set; } 
        public short Short { get; set; }

        [Html5Number]
        public string StringAsNumber { get; set; } 
        
        [Html5Step(5)]
        [Html5UseDefaultMinMax]
        public int Int { get; set; }

        [Html5Range]
        [Html5UseDefaultMinMax]
        public long Long { get; set; } 
        
        [Range(10, 100)]
        public float Float { get; set; } 
        
        public double Double { get; set; } 
        
        [Html5Step(0.5)]
        public decimal Decimal { get; set; } 
    }

    public class StringViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Html5Search]
        public string Search { get; set; }

        [Html5Color]
        public string Color { get; set; }
    }

    public class DateViewModel
    {
        [DataType(DataType.Date)]
        public string Date { get; set; }

        [Html5DateTime]
        public DateTime DateTime { get; set; }

        [DataType(DataType.DateTime)]
        //[Html5DateTimeLocal]
        public DateTime DateTimeLocal { get; set; }
        [Html5Time]
        public string Time { get; set; }
        [Html5Week]
        public string Week { get; set; }
        [Html5Month]
        public string Month { get; set; }
    }
}