using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathDivisionExerciseProvider : IExerciseProvider<MathExercise>
    {
        private Random random { get; }

        private int permutation { get; }

        private int min { get; }

        private int max { get; }

        private MathMultiplyExerciseProvider multProvider { get; }

        public MathDivisionExerciseProvider(Random random, int permutation, int min ,int max)
        {
            this.min = min;
            this.max = max;
            this.permutation = permutation;
            this.random = random;

            this.multProvider = new MathMultiplyExerciseProvider(random, permutation, min, max);
        }

        public MathExercise NextExercise()
        {
            var ex = multProvider.NextExercise();
            int[] numbers = ex.numbers;
            // replace first number with 
            numbers[0] = ex.Solution();

            return new MathDivisionExercise(numbers);
        }
    }
}
