using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class RecipeSearchTerm
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Term { get; set; }
        public virtual Recipe Recipe { get; set; }
    }

}
