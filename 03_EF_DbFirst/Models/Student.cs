using System;
using System.Collections.Generic;

namespace EF_DbFirst.Models
{
    public partial class Student
    {
        /// <summary>
        /// The HashSet<T> class provides high-performance set operations. 
        /// A set is a collection that contains no duplicate elements, and whose elements are in no particular order.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0
        /// </summary>
        public Student()
        {
            CoursesCourses = new HashSet<Course>();
        }

        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }

        public virtual ICollection<Course> CoursesCourses { get; set; }
    }
}
