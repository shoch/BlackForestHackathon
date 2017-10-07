using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class GeographyExercise : IExercise
    {
        private Capital capital;


        public GeographyExercise(Capital capital)
        {
            this.capital = capital;
        }


        public string GetQuestion()
        {
            return "Wie heißt die Hauptstadt von " + capital.country + "?";
        }

        public string GetSolution()
        {
            return "Die Hauptstadt von " + capital.country + " ist " + capital.name;
        }

        public bool ValidateAnswer(string answer)
        {
            return answer.Equals(capital.name);
        }
    }
}
