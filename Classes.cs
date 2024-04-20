using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MauiApp1
{
    public class Classes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; } //enum??
        public string TeacherName { get; set; }
        public string TeacherPhone { get; set; }
        public string TeacherEmail {  get; set; }
        public string Notes { get; set; }
        public bool Notify { get; set; }

    }
}
