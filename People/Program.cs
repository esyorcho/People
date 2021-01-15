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

        static void ShowPersonFullName(PersonDto personDto)
        {
            string message = personDto != null ?
                $"Full Name: {personDto.First} {personDto.Last}" :
            "This person doesn't exist";
            Console.WriteLine(message);
        }

        static void ShowPersonsFirstNames(List<PersonDto> persons)
        {
            Console.WriteLine($"First Names: {string.Join(", ", persons.Select(p => p.First))}");
        }

        static void ShowGenderCountByAge(Dictionary<int, List<Tuple<Enums.Gender, int>>> genderCountByAge)
        {
            foreach (var keyValuePair in genderCountByAge)
            {
                Console.WriteLine(
                    $"Age: {keyValuePair.Key} " +
                    string.Join(" ", keyValuePair.Value.Select(g => $"{g.Item1.ToString()}: {g.Item2.ToString()}"))
                );
            }
        }

        static async Task<List<PersonDto>> GetPersonsAsync(string path)
        {
            List<PersonDto> persons = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                persons = await response.Content.ReadAsAsync<List<PersonDto>>();
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

                // Show full name of personDto with id 42
                var personId42 = allPersons.SingleOrDefault(p => p.Id == 42);
                ShowPersonFullName(personId42);

                // Show all first names of people who are 23:
                var persons23 = allPersons.Where(p => p.Age == 23).ToList(); 
                ShowPersonsFirstNames(persons23);

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
                    }).ToDictionary(g => g.Age, g => g.Genders.ToList());
                ShowGenderCountByAge(gendersPerAgeSorted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}