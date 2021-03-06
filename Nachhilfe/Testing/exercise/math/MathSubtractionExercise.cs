﻿using System;
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

        public override string GetQuestion()
        {
            return "Was ergibt " + string.Join(" minus ", numbers) + "?";
        }

        public override string GetSolution()
        {
            return string.Join(" minus ", numbers) + " ergibt " + Solution().ToString() + ".";
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

        public override bool Validate(int answer)
        {
            return answer == Solution();
        }
    }
}
