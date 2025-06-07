namespace exemplu.Models
{
    public class CONCURENT
    {
        public int Id { get; set; }
        public string? Nume { get; set; }= null!;

        public string? Prenume { get; set; }= null!;
        public DateTime? DataNasterii { get; set; }

        public string? Tara { get; set; } = null!;
        public int? Varsta { get; set; } 
        public int? CONCURSId { get; set; }
        public CONCURS? CONCURS { get; set; } = null!;
       
    }
}
