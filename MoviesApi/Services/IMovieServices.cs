namespace MoviesApi.Services
{
    public interface IMovieServices
    {
        public Task<IEnumerable<Movie>> GetAllAsync(byte genreId=0);
        public Task<Movie> GetByIdAsync(int id);
        public Movie Update(Movie movie);
        public Movie Delete(Movie movie);
        public Task<Movie> CreateAsync(Movie movie);


    }
}
