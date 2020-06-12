using System;
using System.Collections.Generic;

namespace DB.DbModels
{
    public partial class Courses
    {
        public Courses()
        {
            Enrollments = new HashSet<Enrollments>();
        }

        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public ICollection<Enrollments> Enrollments { get; set; }
    }
}
