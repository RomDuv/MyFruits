using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFruits.Data;
using MyFruits.Models;
using MyFruits.Services;

namespace MyFruits.Areas.Fruits.Pages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public EditModel(ApplicationDbContext context, ImageService ImgServ)
        {
            _context = context;
            _imageService = ImgServ;
        }

        [BindProperty]
        public Fruit Fruit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Fruits == null)
            {
                return NotFound();
            }

            var fruit =  await _context.Fruits.Include(f=>f.Image).FirstOrDefaultAsync(m => m.Id == id);
            if (fruit == null)
            {
                return NotFound();
            }
            Fruit = fruit;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var fruitsToUpdate = await _context.Fruits
                .Include(f => f.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if(fruitsToUpdate == null)
                return NotFound();


            var uploadedImage = Fruit.Image;
            if (uploadedImage != null)
            {
                uploadedImage = await _imageService.UploadAsync(uploadedImage);

                if (fruitsToUpdate != null)
                {
                    _imageService.deleteUpoloadedFile(fruitsToUpdate.Image);
                    fruitsToUpdate.Image.Name = uploadedImage.Name;
                    fruitsToUpdate.Image.Path = uploadedImage.Path;
                }
                else
                    fruitsToUpdate.Image = uploadedImage;
            }

            if (await TryUpdateModelAsync(fruitsToUpdate, "fruit", f => f.Name, f => f.Description, f => f.Price))
            {
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }


            return Page();
        }

        private bool FruitExists(int id)
        {
          return (_context.Fruits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
