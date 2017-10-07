using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe.exercise.math.exercise
{
    class MathChainedExercise : MathExercise
    {

        private MathExercise[] exercises { get; }

        public MathChainedExercise(MathExercise[] exercises)
            : base(new int[exercises.Length])
        {
            this.exercises = exercises;

            for (int i = 0; i < exercises.Length; i++)
            {
                numbers[i] = exercises[i].Solution();
            }
        }

        public override string BuildExercises()
        {
            string[] strs = new string[exercises.Length];

            for (int i = 0; i < exercises.Length; i++)
            {
                strs[i] = exercises[i].BuildExercises();
            }

            return " Klammer auf " + string.Join(" Klammer zu plus Klammer auf ", strs) + " Klammer zu ";
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
