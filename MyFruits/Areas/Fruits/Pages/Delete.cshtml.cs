using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFruits.Data;
using MyFruits.Models;
using MyFruits.Services;

namespace MyFruits.Areas.Fruits.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext ctx;
        private readonly ImageService imgServ;


        public DeleteModel(ApplicationDbContext context, ImageService imageService)
        {
            ctx = context;
            imgServ = imageService;
        }

        [BindProperty]
        public Fruit Fruit { get; set; } = new();
        public string errorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? hasErrorMessage)
        {
            if (id == null || ctx.Fruits == null)
            {
                return NotFound();
            }

            var fruit = await ctx.Fruits
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fruit == null)
            {
                return NotFound();
            }
            
            if(hasErrorMessage.GetValueOrDefault()) 
            {
                errorMessage = $"Une errur est survenue lors de la tentative de suppression de {Fruit.Name} ({Fruit.Id})";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || ctx.Fruits == null)
            {
                return NotFound();
            }

            var fruitToDelete = await ctx.Fruits
                .Include(f =>f.Image)
                .FirstOrDefaultAsync(f=>f.Id == id);

            if (fruitToDelete == null)
            {
                return NotFound();
            }

            try
            {
                imgServ.deleteUpoloadedFile(fruitToDelete.Image);
                ctx.Fruits.Remove(fruitToDelete);
                await ctx.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception)
            {

                return RedirectToPage("./Delete", new {id, hasErrorMessage = true});
            }

            
        }
    }
}
