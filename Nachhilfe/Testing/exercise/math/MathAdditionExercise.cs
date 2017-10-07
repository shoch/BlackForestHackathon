using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class MathAdditionExercise : MathExercise
    {

        public MathAdditionExercise(int[] numbers)
            : base(numbers)
        {
            
        }

        public override string GetQuestion()
        {
            return "Was ergibt " + string.Join(" plus ", numbers) + "?";
        }

        public override string GetSolution()
        {
            return string.Join(" plus ", numbers) + " ergibt " + Solution().ToString() + ".";
        }

        public override int Solution()
        {
            int sum = 0;

            foreach (int number in numbers)
            {
                sum += number;
            }

            return sum;
        }

        public override bool ValidateAnswer(int answer)
        {
            return answer == Solution();
        }
    }
}
