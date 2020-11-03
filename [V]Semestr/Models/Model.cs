using System;

namespace _V_Semestr.Models
{
    public class Model
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;

        public bool Shown { get; set; }
    }
}
