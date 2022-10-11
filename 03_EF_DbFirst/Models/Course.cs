using System;
using System.Collections.Generic;

namespace EF_DbFirst.Models
{
    public enum rating { Low, Medium, High }
    public partial class Course
    {
        /// <summary>
        /// The HashSet<T> class provides high-performance set operations. 
        /// A set is a collection that contains no duplicate elements, and whose elements are in no particular order.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0
        /// </summary>
        public Course()
        {
            StudentsStudents = new HashSet<Student>();
            Rating = rating.Medium;
        }

        public int CourseId { get; set; }
        public string Title { get; set; } = null!;

        public rating Rating { get; set; }

        public virtual ICollection<Student> StudentsStudents { get; set; }
    }
}
