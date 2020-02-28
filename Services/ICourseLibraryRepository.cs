using Course.API.Entities;
using Course.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.API.Services
{
    public interface ICourseLibraryRepository
    {
        IEnumerable<Entities.Course> GetCourses(Guid authorId);
        Entities.Course GetCourse(Guid authorId, Guid courseId);
        void AddCourse(Guid authorId, Entities.Course course);
        void UpdateCourse(Entities.Course course);
        void DeleteCourse(Entities.Course course);
        IEnumerable<Author> GetAuthors();
        IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);
        bool Save();
    }
}
