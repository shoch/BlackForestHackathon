using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathAdditionExerciseProvider : IExerciseProvider<MathExercise>
    {
        private Random random { get; }

        private int permutation { get; }

        private int min { get; }

        private int max { get; }

        public MathAdditionExerciseProvider(Random random, int permutation, int min ,int max)
        {
            this.min = min;
            this.max = max;
            this.permutation = permutation;
            this.random = random;
        }

        public MathExercise NextExercise()
        {
            int[] numbers = new int[permutation];
            int tmin = min;
            int tmax = max - ((permutation - 1) * min);

            for (int i = 0; i < permutation; i++)
            {
                int n = random.Next(tmin, tmax);
                numbers[i] = n;
                // correct tmax
                tmax -= (n - min);
            }

            return new MathAdditionExercise(numbers);
        }
    }
}
