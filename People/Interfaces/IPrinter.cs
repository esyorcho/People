using People.Client.DTOs;
using People.Client.Helpers;
using System;
using System.Collections.Generic;

namespace People.Client.Interfaces
{
    public interface IPrinter
    {
        void DisplayNotFound(string message = "any parameter");

        void DisplayPersonFullName(IEnumerable<PersonDto> persons);

        void DisplayPersonsFirstNames (IEnumerable<PersonDto> persons);

        void DisplayGenderCountByAge(Dictionary<int, IEnumerable<Tuple<Enums.Gender, int>>> genderCountByAge);
    }

}
