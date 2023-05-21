using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DockerWebTest.Models;
using Newtonsoft.Json;
using System.Text;

namespace DockerWebTest.Controllers
{
    public class PeopleController : Controller
    {
        private readonly string _url;
        public PeopleController(IConfiguration configuration)
        {
            _url = configuration.GetValue<string>("ApiUrl");
        }
        // GET: People
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = client.GetAsync($"{_url}/api/people").Result;
            var content = await response.Content.ReadAsStringAsync();
            var people = JsonConvert.DeserializeObject<List<Person>>(content);

            return View(people);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Person person)
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/api/people", data);
                var content = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
    }
}
