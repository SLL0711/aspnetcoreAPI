using System;
using System.Collections.Generic;

namespace DB.DbModels
{
    public partial class Students
    {
        public Students()
        {
            Enrollments = new HashSet<Enrollments>();
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EmailAddress { get; set; }

        public ICollection<Enrollments> Enrollments { get; set; }
    }
}
