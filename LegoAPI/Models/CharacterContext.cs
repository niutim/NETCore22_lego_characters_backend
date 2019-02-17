using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LegoAPI.Models
{
    public class CharacterContext : DbContext
    {
        private readonly bool _setDbInMemory = false;

        public CharacterContext(DbContextOptions<CharacterContext> options)
            : base(options)
        {
        }

        public CharacterContext(DbContextOptions<CharacterContext> options, bool setDbInMemory)
    : base(options)
        {
            _setDbInMemory = setDbInMemory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  {
            if (_setDbInMemory) optionsBuilder.UseInMemoryDatabase("CharacterList");
            else base.OnConfiguring(optionsBuilder);
        }

        public DbSet<CharacterItem> CharacterItems { get; set; }

        public List<CharacterItem> CharacterItemsToList() { return CharacterItems.ToList(); }
    }
}