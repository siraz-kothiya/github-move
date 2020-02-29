using AutoMapper;
using Course.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.API.Profiles
{ 
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, CourseDto>();
            CreateMap<CourseForCreationDto, Entities.Course>();
            CreateMap<Entities.Course, CourseForUpdateDto>();
            CreateMap<CourseForUpdateDto,Entities.Course>();
        }
    }
}
