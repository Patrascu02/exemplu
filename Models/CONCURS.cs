namespace exemplu.Models
{
    public class CONCURS
    {

        public int Id { get; set; }

        public string? Nume { get; set; }

        public DateTime? Data { get; set; }

        public string? Categorie { get; set; }

        public int? nr_max_participanti { get; set; }

        public bool? restrictie_varsta { get; set; }

        public ICollection<CONCURENT>? CONCURENTI { get; set; } = new List<CONCURENT>();


    }
}
