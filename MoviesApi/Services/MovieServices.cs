namespace MoviesApi.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _context;

        public MovieServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;

        }

        public async Task<IEnumerable<Movie>> GetAllAsync(byte genreId=0)
        {
           return await _context.Movies
                .Where(m=>m.GenreId == genreId || genreId==0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id) =>
            await _context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.Id==id);


        public Movie Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
            return movie;

        }
    }
}
