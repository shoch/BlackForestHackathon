using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public abstract class MathExercise : IExercise<int>
    {
        public int[] numbers { get; private set; }

        public MathExercise(int[] numbers)
        {
            this.numbers = numbers;
        }

        public abstract string GetQuestion();
        public abstract string GetSolution();
        public abstract int Solution();
        public abstract bool ValidateAnswer(int answer);
    }
}
