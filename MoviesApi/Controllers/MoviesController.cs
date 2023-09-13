using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using MoviesApi.Models;
using MoviesApi.Services;
using AutoMapper;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieServices _movieServices;
        private readonly IGenreServices _genreServices;
        private readonly IMapper _mapper;

        public MoviesController(IMovieServices movieServices, IGenreServices genreServices, IMapper mapper)
        {
            _movieServices = movieServices;
            _genreServices = genreServices;
            _mapper = mapper;
        }
        private readonly List<string> _allowedExtensions = new List<string>() { ".png", ".jpg" };
        private readonly long _allowedMoviePosterSize = 1 * 1024 * 1024;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            var movie = await _movieServices.GetByIdAsync(id);
            if (movie == null)
                return NotFound("Invaild movie id!");
            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(movieDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieServices.GetAllAsync();
                //ToDo map movies to moviesDto
                var data=_mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }
        [HttpGet("GetByGenreId/{GenreId}")]
        public async Task<IActionResult> GetByGenreIdAsync(byte GenreId)
        {

            if (!await _genreServices.VaildGenre(GenreId))
                return BadRequest("Invaild genre id!");
            var movies=await _movieServices.GetAllAsync(GenreId);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto movieDto) 
        {
            if (movieDto.Poster == null)
                return BadRequest("Poster is Required!");
            if (!_allowedExtensions.Contains(Path.GetExtension(movieDto.Poster.FileName)))
                return BadRequest("Only .png , .jpg are allowed for poster.");
            if (_allowedMoviePosterSize < movieDto.Poster.Length)
                return BadRequest("The max allowed size for poster is 1Mb.");
            
            if (! await _genreServices.VaildGenre(movieDto.GenreId))
                return BadRequest("Invaild genre id!");
       
                using var data = new MemoryStream();
                await movieDto.Poster.CopyToAsync(data);

                var movie = _mapper.Map<Movie>(movieDto);
                movie.Poster=data.ToArray();

                await _movieServices.CreateAsync(movie);

                return Ok(movie);
           
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id,[FromForm]MovieDto movieDto)
        {
            var movie =await _movieServices.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"No Movie was found by ID: {id}");
            else
            {
                
                if (! await _genreServices.VaildGenre(movieDto.GenreId))
                    return BadRequest("Invaild genre id!");
                if (movieDto.Poster != null)
                {
                    if (!_allowedExtensions.Contains(Path.GetExtension(movieDto?.Poster.FileName)))
                        return BadRequest("Only .png , .jpg are allowed for poster.");
                    if (_allowedMoviePosterSize < movieDto.Poster.Length)
                        return BadRequest("The max allowed size for poster is 1Mb.");


                    using var datastream = new MemoryStream();
                    await movieDto.Poster.CopyToAsync(datastream);

                    movie.Poster = datastream.ToArray();
                }
                movie.Title = movieDto.Title;
                movie.Year = movieDto.Year;
                movie.StoryLine = movieDto.StoryLine;
                movie.Rate = movieDto.Rate;
                movie.GenreId = movieDto.GenreId;

                _movieServices.Update(movie);

             
                return Ok(movie);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _movieServices.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"No movie was found by Id:{id}");
            _movieServices.Delete(movie);
            return Ok(movie);

        }

    }
}
