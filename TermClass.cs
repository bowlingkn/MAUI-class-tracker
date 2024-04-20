using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MauiApp1
{
    public class TermClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name {  get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
    }
}
