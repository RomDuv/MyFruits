using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFruits.Data;
using MyFruits.Models;

namespace MyFruits.Areas.Fruits.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyFruits.Data.ApplicationDbContext _context;

        public IndexModel(MyFruits.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Fruit> Fruit { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Fruits != null)
            {
                Fruit = await _context.Fruits.Include(f => f.Image).ToListAsync();

            }
        }
    }
}
