using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using TodoApi.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [TempData]
    public string? RegistrationMessage { get; set; }

    [BindProperty]
    public UserLoginDto LoginData { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("Login page loaded");
        if (!string.IsNullOrEmpty(RegistrationMessage))
        {
            _logger.LogInformation("Registration message: {Message}", RegistrationMessage);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("Login attempt for username: {Username}", LoginData.Username);

        // Log the raw form data for debugging
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation("Form data - {Key}: {Value}", key, Request.Form[key]);
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login failed: Model validation failed");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Validation error: {Error}", error.ErrorMessage);
            }
            ErrorMessage = "Please fill in all required fields correctly.";
            return Page();
        }

        try
        {
            _logger.LogInformation("Sending login request to API");
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/auth/login", LoginData);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Login successful for user: {Username}", LoginData.Username);
                var responseData = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (responseData != null && !string.IsNullOrEmpty(responseData.Token))
                {
                    Response.Cookies.Append("AuthToken", responseData.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Set to true in production
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });
                }
                else
                {
                    _logger.LogWarning("Login API did not return a token");
                    ErrorMessage = "Login failed: no token received.";
                    return Page();
                }
                return RedirectToPage("/Task/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Login failed with status code: {StatusCode}, Error: {Error}", 
                    response.StatusCode, errorContent);
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during login for user: {Username}", LoginData.Username);
            ErrorMessage = "An error occurred. Please try again.";
            return Page();
        }
    }
}
