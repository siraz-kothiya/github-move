using Course.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if(validationContext.ObjectInstance is IEnumerable<CourseForCreationDto>)
            {
                var courses = (IEnumerable<CourseForCreationDto>)validationContext.ObjectInstance;

                foreach(var course in courses)
                {
                    if (course.Title == course.Description)
                    {
                        return new ValidationResult(ErrorMessage,
                            new[] { nameof(CourseForManipulationDto) });
                    }
                }
            }
            else
            {
                var course = (CourseForManipulationDto)validationContext.ObjectInstance;

                if (course.Title == course.Description)
                {
                    return new ValidationResult(ErrorMessage,
                        new[] { nameof(CourseForManipulationDto) });
                }
            }
            
            return ValidationResult.Success;
        }
    }
}
