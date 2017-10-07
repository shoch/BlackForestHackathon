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

        public override string GetQuestion()
        {
            return "Was ergibt " + string.Join(" mal ", numbers) + "?";
        }

        public override string GetSolution()
        {
            return string.Join(" mal ", numbers) + " ergibt " + Solution().ToString() + ".";
        }

        public override int Solution()
        {
            int sum = 0;

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
