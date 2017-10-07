using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe.exercise.math.provider.complex
{
    public class MathRandomExerciseProvider : IExerciseProvider<MathExercise>
    {
        private Random random { get; }

        private IExerciseProvider<MathExercise>[] exerciseProvider { get; }

        public MathRandomExerciseProvider(IExerciseProvider<MathExercise>[] exerciseProvider)
        {
            this.exerciseProvider = exerciseProvider;
        }

        public MathExercise NextExercise()
        {
            // get one provider randomly
            return exerciseProvider[random.Next(0, exerciseProvider.Length)].NextExercise();
        }
    }
}
