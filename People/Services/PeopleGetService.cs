using People.Client.DTOs;
using People.Client.Helpers;
using People.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace People.Client.Services
{
    public class PeopleGetService : IPeopleGetService
    {
        private static HttpClient _httpClient = new HttpClient();
        private const string baseAddress = "https://[sampleTestUrl]";
        private IPrinter _printer;

        public PeopleGetService(IPrinter printer)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _printer = printer;
        }

        public async Task RunTasks(IPrinter printer)
        {
            _printer = printer;
            IEnumerable<PersonDto> allPersons = null;

            try
            {
                // Get all people
                allPersons = await GetPersonsAsync(baseAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            DisplayFullNameOfPeopleWithId(allPersons);

            DisplayFirstNamesOfPeopleWithAge(allPersons);

            DisplayNumberGendersPerAgeOrdered(allPersons);
        }

        public async Task<IEnumerable<PersonDto>> GetPersonsAsync(string path)
        {
            IEnumerable<PersonDto> persons = null;
            HttpResponseMessage response = await _httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                persons = await response.Content.ReadAsAsync<IEnumerable<PersonDto>>();
                return persons;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Data not found");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorised access");
            }
            else
            {
                Console.WriteLine($"Error status code: {response.StatusCode}");
            }

            return persons ?? new PersonDto[0];
        }

        // Show full name of person with id 42
        private void DisplayFullNameOfPeopleWithId(IEnumerable<PersonDto> allPersons, int id = 42)
        {
            var persons = allPersons.Where(p => p.Id == id);

            if (persons.Any())
            {
                _printer.DisplayPersonFullName(persons);
            }
            else
            {
                _printer.DisplayNotFound($"id {id}");
            }
        }

        // Show all first names of people who are 23:
        private void DisplayFirstNamesOfPeopleWithAge(IEnumerable<PersonDto> allPersons, int age = 23)
        {
            var persons = allPersons.Where(p => p.Age == age);

            if (persons.Any())
            {
                _printer.DisplayPersonsFirstNames(persons);
            }
            else
            {
                _printer.DisplayNotFound($"Age {age}");
            }
        }

        private void DisplayNumberGendersPerAgeOrdered(IEnumerable<PersonDto> allPersons)
        {
            // Number of genders per age, from youngest to oldest
            var gendersPerAgeSorted = allPersons
                .GroupBy(p => p.Age)
                .OrderBy(a => a.Key)
                .Select(a => new
                {
                    Age = a.Key,
                    Genders = a.GroupBy(g => g.Gender)
                        .Select(genderGroup => new Tuple<Enums.Gender, int>
                        (
                            genderGroup.Key,
                            genderGroup.Count()
                        ))
                }).ToDictionary(g => g.Age, g => g.Genders.AsEnumerable());

            if (gendersPerAgeSorted.Any())
            {
                _printer.DisplayGenderCountByAge(gendersPerAgeSorted);
            }
            else
            {
                _printer.DisplayNotFound();
            }
        }
    }
}
