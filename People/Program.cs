using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using People.Client.DTOs;
using People.Client.Helpers;

namespace People.Client
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private const string baseAddress = "https://f43qgubfhf.execute-api.ap-southeast-2.amazonaws.com/sampletest/";

        static async Task<IEnumerable<PersonDto>> GetPersonsAsync(string path)
        {
            IEnumerable<PersonDto> persons = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                persons = await response.Content.ReadAsAsync<IEnumerable<PersonDto>>();
            }

            return persons;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get all people
                var allPersons = await GetPersonsAsync(baseAddress);

                RunQueries(allPersons);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        static void RunQueries(IEnumerable<PersonDto> allPersons)
        {
            DisplayFullNameOfPeopleWithId(allPersons);

            DisplayFirstNamesOfPeopleWithAge(allPersons);

            DisplayNumberGendersPerAgeOrdered(allPersons);
        }

        // Show full name of person with id 42
        static void DisplayFullNameOfPeopleWithId(IEnumerable<PersonDto> allPersons, int id = 42)
        {
            var persons = allPersons.Where(p => p.Id == id);
            if (persons.Any())
            {
                Print.DisplayPersonFullName(persons);
            }
            else
            {
                Print.DisplayNotFound($"id {id}");
            }
        }

        // Show all first names of people who are 23:
        static void DisplayFirstNamesOfPeopleWithAge(IEnumerable<PersonDto> allPersons, int age = 23)
        {
            var persons = allPersons.Where(p => p.Age == age);
            if (persons.Any())
            {
                Print.DisplayPersonsFirstNames(persons);
            }
            else
            {
                Print.DisplayNotFound($"Age {age}");
            }
        }

        static void DisplayNumberGendersPerAgeOrdered(IEnumerable<PersonDto> allPersons)
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
                Print.DisplayGenderCountByAge(gendersPerAgeSorted);
            }
            else
            {
                Print.DisplayNotFound();
            }
        }
    }
}