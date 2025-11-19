using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;


namespace Recipe_Finder
{
    public class ThemeService
    {
        public AppTheme CurrentTheme { get; private set; } = AppTheme.Light;

        public event Action ThemeChanged;

        public void SetTheme(AppTheme theme)
        {
            CurrentTheme = theme;
            ThemeChanged?.Invoke();
        }

        public void ToggleTheme()
        {
            CurrentTheme = CurrentTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            ThemeChanged?.Invoke();
        }
    }

}
