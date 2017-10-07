using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathMultiplyExercise : MathExercise
    {

        public MathMultiplyExercise(int[] numbers)
            : base(numbers)
        {

        }

        public override string BuildExercises()
        {
            return string.Join(" mal ", numbers);
        }

        public override int Solution()
        {
            int sum = 1;

            foreach (int number in numbers)
            {
                sum *= number;
            }

            return sum;
        }

        public override bool Validate(int answer)
        {
            return answer == Solution();
        }
    }
}
