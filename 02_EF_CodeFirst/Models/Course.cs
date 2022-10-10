using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace EF_CodeFirst.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        [Required]
        [StringLength(60)]
        public string Title { get; set; }

        //For StudentCourse collection/table
        //Entity includes a collection navigation property of type ICollection<Student>
        //This also results in a one-to-many relationship between the Student and Course entities. 
        public ICollection<Student>? Students { get; set; }

        //OR
        //one-to-many relationship between the Student and Course
        //where many students are associated with one  Course
        //public Course Course { get; set; }

        /// <summary>
        /// Or set a Key manually as below
        /// </summary>
        //public int StudentId { get; set; }
        //[ForeignKey("StudentId")]
        //public virtual Student Student { get; set; }
    }

}
