using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TodoApi.Pages.TaskItems
{
    public class ListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            // Send the JWT token in the Authorization header if it exists in the cookie
            if (Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            Tasks = await client.GetFromJsonAsync<List<TaskItem>>("api/Tasks");
        }
    }
}
