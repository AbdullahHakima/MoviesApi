namespace MoviesApi.Services
{
    public class GenreServices : IGenreServices
    {
        private readonly ApplicationDbContext _context;
        public GenreServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task DeleteAsync(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()=>
            await _context.Genres.OrderBy(g=>g.Name).ToListAsync();


        public async Task<Genre> GetById(byte id) => 
            await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);


        public async Task<Genre> UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<bool> VaildGenre(byte id)
        {
            return await _context.Genres.AnyAsync(g => g.Id == id);
        }
    }
}
