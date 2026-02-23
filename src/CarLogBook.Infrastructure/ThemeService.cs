namespace CarLogBook.Infrastructure;

public class ThemeService
{
    public bool IsDarkMode { get; private set; }

    public string CurrentTheme => IsDarkMode ? "dark" : "light";

    public event Action? OnThemeChanged;

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnThemeChanged?.Invoke();
    }
}
