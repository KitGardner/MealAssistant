using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Pages.Ingredients
{
    public class DetailsModel : PageModel
    {
        private readonly MealAssistant.Data.AppDbContext _context;

        public DetailsModel(MealAssistant.Data.AppDbContext context)
        {
            _context = context;
        }

        public Ingredient Ingredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(m => m.Id == id);

            if (ingredient is not null)
            {
                Ingredient = ingredient;

                return Page();
            }

            return NotFound();
        }
    }
}
