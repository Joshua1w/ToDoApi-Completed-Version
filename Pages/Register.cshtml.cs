using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using TodoApi.Dtos;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace TodoApi.Pages;

public class RegisterModel : PageModel
{
    private readonly ILogger<RegisterModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    [TempData]
    public string? RegistrationMessage { get; set; }

    public RegisterModel(IHttpClientFactory httpClientFactory, ILogger<RegisterModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("Register page loaded");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("Registration attempt for username: {Username}", Username);

        // Log the raw form data for debugging
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation("Form data - {Key}: {Value}", key, Request.Form[key]);
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Registration failed: Model validation failed");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Validation error: {Error}", error.ErrorMessage);
            }
            return Page();
        }

        try
        {
            var registerData = new UserRegisterDto
            {
                Username = Username.Trim(),
                Password = Password
            };

            _logger.LogInformation("Sending registration request to API");
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/auth/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Registration successful for user: {Username}", Username);
                RegistrationMessage = "Registration successful! Please log in.";
                return RedirectToPage("/Login");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Registration failed with status code: {StatusCode}, Error: {Error}", 
                    response.StatusCode, errorContent);
                Message = $"Registration failed: {errorContent}";
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during registration for user: {Username}", Username);
            Message = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}