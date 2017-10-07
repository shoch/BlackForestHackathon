using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public abstract class MathExercise : IExercise
    {
        public int[] numbers { get; private set; }

        public MathExercise(int[] numbers)
        {
            this.numbers = numbers;
        }


        public bool ValidateAnswer(string answer)
        {
            return Validate(Convert.ToInt32(answer));
        }

        public string GetQuestion()
        {
            return "Was ergibt " + BuildExercises() + "?";
        }

        public string GetSolution()
        {
            return BuildExercises() + " ergibt " + Solution().ToString() + ".";
        }

        public abstract string BuildExercises();
        public abstract int Solution();
        public abstract bool Validate(int answer);
    }
}
