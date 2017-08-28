using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextClassification.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TextClassification.DAL
{
    public class TextContext : DbContext
    {
        public TextContext() : base("TextContext")
        {
        }

        public DbSet<Text> Texts { get; set; }
    }
}