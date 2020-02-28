using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Course.API.Entities;
using Course.API.Models;
using Course.API.ResourceParameters;
using Course.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(
            [FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var authors = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpGet("{authorId}",Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var author = _courseLibraryRepository.GetAuthor(authorId);

            if(author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var author = _mapper.Map<Author>(authorForCreationDto);
            _courseLibraryRepository.AddAuthor(author);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(author);
            return CreatedAtRoute("GetAuthor", new { authorId = author.Id }, authorToReturn);
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorToDelete = _courseLibraryRepository.GetAuthor(authorId);

            if(authorToDelete == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorToDelete);
            _courseLibraryRepository.Save();

            return NoContent();
        }
    }
}