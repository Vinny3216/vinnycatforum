using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vinnycatforum.Models;

namespace vinnycatforum.Data
{
    public class vinnycatforumContext : DbContext
    {
        public vinnycatforumContext (DbContextOptions<vinnycatforumContext> options)
            : base(options)
        {
        }

        public DbSet<vinnycatforum.Models.Comment> Comment { get; set; } = default!;
        public DbSet<vinnycatforum.Models.Discussion> Discussion { get; set; } = default!;
    }
}
