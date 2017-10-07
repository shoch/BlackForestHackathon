using Nachhilfe.exercise.math.exercise;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe.exercise.math.provider.complex
{
    class MathChainedExerciseProvider
    {
        private IExerciseProvider<MathExercise>[] exerciseProvider { get; }

        public MathChainedExerciseProvider(IExerciseProvider<MathExercise>[] exerciseProvider)
        {
            this.exerciseProvider = exerciseProvider;
        }

        public MathExercise NextExercise()
        {
            MathExercise[] ary = new MathExercise[exerciseProvider.Length];

            for (int i = 0; i < exerciseProvider.Length; i++)
            {
                ary[i] = exerciseProvider[i].NextExercise();
            }

            return new MathChainedExercise(ary);
        }
    }
}
