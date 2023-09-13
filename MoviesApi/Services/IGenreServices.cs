namespace MoviesApi.Services
{
    public interface IGenreServices
    {

        public Task<IEnumerable<Genre>> GetAllAsync();
        public Task<Genre> GetById(byte id);
        public Task<Genre> UpdateAsync(Genre genre);
        public Task DeleteAsync(Genre genre);
        public Task<Genre> AddAsync(Genre genre);
        public Task<bool> VaildGenre(byte id);

    }
}
