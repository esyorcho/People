using People.Client.DTOs;
using People.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using People.Client.Interfaces;

namespace People.Client
{
    public class PrintToConsole : IPrinter
    {
        public void DisplayNotFound(string message = "any parameter")
        {
            Console.WriteLine($"Nobody was found with {message}");
        }

        public void DisplayPersonFullName(IEnumerable<PersonDto> persons)
        {
            Console.WriteLine($"Full Name with id {persons.First().Id}: {string.Join(", ", persons.Select(p => $"{p.First} {p.Last}"))}");
        }

        public void DisplayPersonsFirstNames(IEnumerable<PersonDto> persons)
        {
            Console.WriteLine($"First Names with age {persons.First().Age}: {string.Join(", ", persons.Select(p => p.First))}");
        }

        public void DisplayGenderCountByAge(Dictionary<int, IEnumerable<Tuple<Enums.Gender, int>>> genderCountByAge)
        {
            foreach (var keyValuePair in genderCountByAge)
            {
                Console.WriteLine(
                    $"Age: {keyValuePair.Key} " +
                    string.Join(" ", keyValuePair.Value.Select(g => $"{g.Item1.GetDescription()}: {g.Item2.ToString()}"))
                );
            }
        }
    }
}
