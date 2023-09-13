namespace MoviesApi.Dtos
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(2500)]
        public string StoryLine { get; set; }

        public int Year { get; set; }

        public byte[] Poster { get; set; }

        public double Rate { get; set; }

        public string GenreName { get; set; }

        public int GenreId { get; set; }
    }
}
