using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathSubtractionExercise : MathExercise
    {

        public MathSubtractionExercise(int[] numbers)
            : base(numbers)
        {

        }

        public override string BuildExercises()
        {
            return string.Join(" minus ", numbers);
        }

        public override int Solution()
        {
            int sum = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                sum -= numbers[i];
            }

            return sum;
        }

        public override bool Validate(int answer)
        {
            return answer == Solution();
        }
    }
}
