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
    public class IndexModel : PageModel
    {
        private readonly MealAssistant.Data.AppDbContext _context;

        public IndexModel(MealAssistant.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Ingredient> Ingredient { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Ingredient = await _context.Ingredients.ToListAsync();
        }
    }
}
