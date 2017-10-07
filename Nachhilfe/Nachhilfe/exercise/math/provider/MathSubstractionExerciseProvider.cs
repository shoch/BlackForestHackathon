using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathSubstractionExerciseProvider : IExerciseProvider<MathExercise>
    {
        private Random random { get; }

        private int permutation { get; }

        private int min { get; }

        private int max { get; }

        private int firstMin { get; }

        public MathSubstractionExerciseProvider(Random random, int permutation, int min, int max)
        {
            // has to be larger than min to subtract min later
            // sample :
            // -> 8 = 4 * 2 
            // => 8 - (4*3) = 2 
            // => 2 == min
            firstMin = permutation * min;

            if (max < firstMin)
            {
                throw new Exception("Max should be larger than: " + firstMin);
            }

            this.min = min;
            this.max = max;
            this.permutation = permutation;
            this.random = random;
        }

        public MathExercise NextExercise()
        {
            int[] numbers = new int[permutation];

            int first = random.Next(firstMin, max);
            numbers[0] = first;

            MathAdditionExerciseProvider pAddExercise = new MathAdditionExerciseProvider(random, permutation-1, min, first - min);

            var sum = pAddExercise.NextExercise();

            for (int i = 1; i < permutation; i++)
            {
                numbers[i] = sum.numbers[i-1];
            }

            return new MathSubtractionExercise(numbers);
        }
    }
}
