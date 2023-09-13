using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreServices _genreServices;

        public GenresController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        //CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres=await _genreServices.GetAllAsync();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreDto genreDto)
        {
            var genre = new Genre { Name = genreDto.Name };
            if(ModelState.IsValid)
            {
               await _genreServices.AddAsync(genre);
                return Ok(genre);  
            
            }
            return BadRequest(ModelState.ErrorCount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] byte id)
        {
            var genre = await _genreServices.GetById(id);
            if(genre!=null)
            {
               await _genreServices.DeleteAsync(genre);
                return Ok(genre );
            }
            return NotFound($"No genre was found with ID:{id}");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] byte id, [FromBody] Genre _genre)
        {
            var genre=await _genreServices.GetById(id);
            if (genre!=null)
            {
                genre.Name = _genre.Name;
                await _genreServices.UpdateAsync(genre);
                return Ok(genre);
            }
            return NotFound($"No genre was found with ID:{id}");
        }
    }
}
