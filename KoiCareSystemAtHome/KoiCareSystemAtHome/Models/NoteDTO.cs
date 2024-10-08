namespace KoiCareSystemAtHome.Models
{
    public class NoteDTO
    {
        public int NoteId { get; set; }

        public string? NoteName { get; set; }

        public string NoteText { get; set; } = null!;

        public int AccId { get; set; }
    }
}
