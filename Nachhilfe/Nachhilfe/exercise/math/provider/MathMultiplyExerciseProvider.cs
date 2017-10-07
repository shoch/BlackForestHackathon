using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathMultiplyExerciseProvider : IExerciseProvider<MathExercise>
    {
        private Random random { get; }

        private int permutation { get; }

        private int min { get; }

        private int max { get; }

        public MathMultiplyExerciseProvider(Random random, int permutation, int min ,int max)
        {
            this.min = min;
            this.max = max;
            this.permutation = permutation;
            this.random = random;
        }

        public MathExercise NextExercise()
        {
            int[] numbers = new int[permutation];

            for (int i = 0; i < permutation; i++)
            {
                numbers[i] = random.Next(min, max);
            }

            return new MathDivisionExercise(numbers);
        }
    }
}
