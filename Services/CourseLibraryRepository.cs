using Course.API.DbContexts;
using Course.API.Entities;
using Course.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.API.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly CourseLibraryContext _context;
        public CourseLibraryRepository(CourseLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddAuthor(Author author)
        {
            if(author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            author.Id = Guid.NewGuid();
            foreach(var course in author.Courses)
            {
                course.Id = Guid.NewGuid();   
            }
            _context.Authors.Add(author);

        }

        public void AddCourse(Guid authorId, Entities.Course course)
        {
            if(authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if(course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            course.AuthorId = authorId;
            _context.Courses.Add(course);
        }

        public bool AuthorExists(Guid authorId)
        {
            if(authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if(author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }

        public void DeleteCourse(Entities.Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            _context.Courses.Remove(course);
        }

        public Author GetAuthor(Guid authorId)
        {
            if (authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();

        }

        public IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters)
        {
            if (authorsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(authorsResourceParameters));
            }

            if(string.IsNullOrEmpty(authorsResourceParameters.MainCategory)
                && string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                return GetAuthors();
            }

            var collection = _context.Authors as IQueryable<Author>;

            if (!string.IsNullOrEmpty(authorsResourceParameters.MainCategory))
            {
                var mainCategory = authorsResourceParameters.MainCategory.Trim();
                collection = collection.Where(a => a.MainCategory == mainCategory);
            }

            if (!string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
            {

                var searchQuery = authorsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
                    || a.FirstName.Contains(searchQuery)
                    || a.LastName.Contains(searchQuery));
            }

            return collection.ToList();
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
                ;
        }

        public Entities.Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (courseId == null)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
                .Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Entities.Course> GetCourses(Guid authorId)
        {
            if (authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses.Where(c => c.AuthorId == authorId)
                .OrderBy(c => c.Title)
                .ToList<Entities.Course>();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAuthor(Author author)
        {
            
        }

        public void UpdateCourse(Entities.Course course)
        {
            throw new NotImplementedException();
        }
    }
}
