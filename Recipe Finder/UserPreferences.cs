using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class UserPreferences
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsDairyFree { get; set; }
        public bool IsLowCarb { get; set; }
    }
}
