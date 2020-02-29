using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.API.Models
{
    public class AuthorForCreationDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        [Required]
        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Courses { get; set; }
          = new List<CourseForCreationDto>();
    }
}
