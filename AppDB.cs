using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Android.Preferences;
using SQLite;
using System.IO;
//using static Java.Util.Jar.Attributes;

namespace MauiApp1
{
    public static class AppDB
    {
        private static SQLiteAsyncConnection _db;

        static async Task Init()
        {
            if (_db != null) { return; }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Terms.db");

            _db = new SQLiteAsyncConnection(databasePath);
            
            await _db.CreateTableAsync<TermClass>();
            await _db.CreateTableAsync<Classes>();
            await _db.CreateTableAsync<AssessmentsClass>();
        }

        #region sampleData
        public static async void LoadStartData()
        {
            await Init();

            TermClass term1 = new TermClass()
            {
                Name = "Term 1",
                Start = DateTime.Now.Date,
                End = DateTime.Today.Date
            };
            await _db.InsertAsync(term1);

            Classes class1 = new Classes()
            {
                TermId = term1.Id,
                Name = "class 1",
                Start = DateTime.Today.Date,
                End = DateTime.Today.Date,
                Status = "In Progress",
                TeacherName = "Anika Patel",
                TeacherPhone = "555-123-4567",
                TeacherEmail = "anika.patel@strimeuniversity.edu",
                Notes = "so many notes",
                Notify = true
            };
            await _db.InsertAsync(class1);

            AssessmentsClass assessment1 = new AssessmentsClass()
            {
                ClassId = class1.Id,
                Name = "assessment 1",
                Type = "Performance Assessment",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1)
            };
            await _db.InsertAsync(assessment1);

            AssessmentsClass assessment2 = new AssessmentsClass()
            {
                ClassId = class1.Id,
                Name = "assessment 1",
                Type = "Objective Assessment",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1)
            };
            await _db.InsertAsync(assessment2);

        }

    /*    public static async void LoadStartData() 
        { 
            await Init();

            #region term1
            TermClass term1 = new TermClass()
            {
                Name = "Term 1",
                Start = DateTime.Now.Date,
                End = DateTime.Today.Date
            };
            await _db.InsertAsync(term1);

            Classes class1 = new Classes()
            {
                TermId = term1.Id,
                Name = "class 1",
                Start = DateTime.Today.Date,
                End = DateTime.Today.Date,
                Status = "true",
                TeacherName = "prof 1",
                TeacherPhone = "555-6798",
                TeacherEmail = "blah@wgu.edu",
                Notes = "so many notes",
                Notify = false
            };
            await _db.InsertAsync(class1);

            AssessmentsClass assessment1 = new AssessmentsClass()
            {
                ClassId = class1.Id,
                Name = "assessment 1",
                Type = "Performance Assessment",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 2, 1)
            };
            await _db.InsertAsync(assessment1);

            Classes class2 = new Classes()
            {
                TermId = term1.Id,
                Name = "class 2",
                Start = DateTime.Today.Date,
                End = DateTime.Today.Date,
                Status = "false",
                TeacherName = "prof 2",
                TeacherPhone = "555-0000",
                TeacherEmail = "blah@wgu.edu",
                Notes = "so many notes. so many notes",
                Notify = true
            };
            await _db.InsertAsync(class2);
            #endregion

            #region term 2
            TermClass term2 = new TermClass()
            {
                Name = "Term 2",
                Start = DateTime.Now.Date,
                End = DateTime.Today.Date
            };
            await _db.InsertAsync(term2);

            Classes class3 = new Classes()
            {
                TermId = term2.Id,
                Name = "class 3",
                Start = DateTime.Today.Date,
                End = DateTime.Today.Date,
                Status = "true",
                TeacherName = "prof 1",
                TeacherPhone = "555-6798",
                TeacherEmail = "blah@wgu.edu",
                Notes = "so many notes",
                Notify = true
            };
            await _db.InsertAsync(class3);

            #endregion

            #region term3
            TermClass term3 = new TermClass()
            {
                Name = "Term 3",
                Start = DateTime.Now.Date,
                End = DateTime.Today.Date
            };
            await _db.InsertAsync(term3);

            Classes class4 = new Classes()
            {
                TermId = term3.Id,
                Name = "class 5",
                Start = DateTime.Today.Date,
                End = DateTime.Today.Date,
                Status = "true",
                TeacherName = "prof 1",
                TeacherPhone = "555-6798",
                TeacherEmail = "blah@wgu.edu",
                Notes = "so many notes",
                Notify = false
            };
            await _db.InsertAsync(class4); 

            
        } */
        public static async Task ClearStartData() 
        { 
            await Init();

            await _db.DropTableAsync<TermClass>();
            await _db.DropTableAsync<Classes>();
            await _db.DropTableAsync<AssessmentsClass>();
            _db = null;
        }
        
        #endregion

        #region Terms methods
        public static async Task AddTerm(string name, DateTime start, DateTime end)
        {
            await Init();
            var term = new TermClass()
            {
                Name = name,
                Start = start,
                End = end
            };

            await _db.InsertAsync(term);
            var id = term.Id;   
        }
        public static async Task RemoveTerm(int id) 
        {
            await Init();
            await _db.DeleteAsync<TermClass>(id);
        }
        public static async Task<IEnumerable<TermClass>> GetTerms() 
        {
            await Init();
            var terms = await _db.Table<TermClass>().ToListAsync();
            return terms;
        }
        public static async Task UpdateTerm(int id, string name, DateTime start, DateTime end)
        {
            await Init();
            var termQuery = await _db.Table<TermClass>()
                .Where(i=>i.Id == id).FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = name;
                termQuery.Start = start;
                termQuery.End = end;

                await _db.UpdateAsync(termQuery);
            }
        }
        #endregion

        #region Classes methods
        public static async Task AddClass(int termId, string name, DateTime start, DateTime end, string status, string teacherName, string teacherPhone, string teacherEmail, string notes, bool notify)
        {
            await Init();

            var newClass = new Classes
            {
                TermId = termId,
                Name = name,
                Start = start,
                End = end,
                Status = status,
                TeacherName = teacherName,
                TeacherPhone = teacherPhone,
                TeacherEmail = teacherEmail,
                Notes = notes,
                Notify = notify
            };

            await _db.InsertAsync(newClass);

            var id = newClass.Id;
        }
        public static async Task RemoveClass(int id)
        {
            await Init();
            await _db.DeleteAsync<Classes>(id);
        }

        public static async Task<IEnumerable<Classes>> GetClasses()
        {
            await Init();
            var classes = await _db.Table<Classes>().ToListAsync();
            return classes;
        }

        public static async Task<IEnumerable<Classes>> GetClass(int termId)
        {
            await Init();
            var classes = await _db.Table<Classes>().Where(i => i.TermId == termId).ToListAsync();
            return classes;
        }

        public static async Task<IEnumerable<Classes>> GetSelectedClass(int classId)
        {
            await Init();
            var classes = await _db.Table<Classes>().Where(i => i.Id == classId).ToListAsync();
            return classes;
        }

        public static async Task UpdateClass(int id, string name, DateTime start, DateTime end, string status, string teacherName, string teacherPhone, string teacherEmail, string notes, bool notify)
        {
            await Init();

            var classQuery = await _db.Table<Classes>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (classQuery != null)
            {
                classQuery.Name = name;
                classQuery.Start = start;
                classQuery.End = end;
                classQuery.Status = status;
                classQuery.TeacherName = teacherName;
                classQuery.TeacherPhone = teacherPhone;
                classQuery.TeacherEmail = teacherEmail;
                classQuery.Notes = notes;
                classQuery.Notify = notify;

                await _db.UpdateAsync(classQuery);
            }
        }

        #endregion
        //--------------------------------------
        public static async Task AddAssessment(int classId, string name, string type, DateTime start, DateTime end, bool notify)
        {
            await Init();

            var newAssessment = new AssessmentsClass
            {
                ClassId = classId,
                Name = name,
                Type = type,
                Start = start,
                End = end,
                Notify = notify
            };

            await _db.InsertAsync(newAssessment);

            var id = newAssessment.Id;
        }
        public static async Task RemoveAssessment(int id)
        {
            await Init();
            await _db.DeleteAsync<AssessmentsClass>(id);
        }

        public static async Task<IEnumerable<AssessmentsClass>> GetAssessments()
        {
            await Init();
            var assessments = await _db.Table<AssessmentsClass>().ToListAsync();
            return assessments;
        }

        public static async Task<List<AssessmentsClass>> GetAssessments(int classId)
        {
            await Init();
            var assessments = await _db.Table<AssessmentsClass>().Where(i => i.ClassId == classId).ToListAsync();
            
            return assessments;
        }

        public static async Task UpdateAssessment(int id, string name, string type, DateTime start, DateTime end, bool notify)
        {
            await Init();

            var assessmentQuery = await _db.Table<AssessmentsClass>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Name = name;
                assessmentQuery.Type = type;
                assessmentQuery.Start = start;
                assessmentQuery.End = end;
                assessmentQuery.Notify = notify;

                await _db.UpdateAsync(assessmentQuery);
            }
        }
        public static async Task<bool> GetPerf(int classId)
        {
            await Init();
            // return await _db.Table<AssessmentsClass>().Where(i => i.Type).ToListAsync();
          //   return await _db.QueryAsync<AssessmentsClass>(&"SELECT Type FROM AssessmentsClass WHERE id == {classId} ");
            try
            {
                var assessments = await _db.Table<AssessmentsClass>().Where(i => (i.ClassId == classId) & i.Type.StartsWith("P")).ToListAsync();

                //var result = await _db.Table<AssessmentsClass>().Where(s => s.Type.StartsWith("P")).ToListAsync();
                return true;
            }
            catch { return false; }

        }
        public static async Task<bool> GetObj(int classId)
        {
            await Init();
            // return await _db.Table<AssessmentsClass>().Where(i => i.Type).ToListAsync();
            // return await _db.QueryAsync<AssessmentsClass>("SELECT Type FROM [AssessmentsClass]");
            var result = await _db.Table<AssessmentsClass>().Where(i => (i.ClassId == classId) & i.Type.StartsWith("P")).ToListAsync();
            if (result != null)
                { return true; }
            else
            {
                return false;
            }



            

        }

    }
}
