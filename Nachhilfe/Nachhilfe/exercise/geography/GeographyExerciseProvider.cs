using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public class GeographyExerciseProvider : IExerciseProvider<GeographyExercise>
    {
        private Random random { get; }


        public GeographyExerciseProvider(Random random)
        {
            this.random = random;
        }


        public GeographyExercise NextExercise()
        {
            IList<Capital> capitals = Capitals.GetCapitals();
            int capitalIndex = random.Next(capitals.Count);
            return new GeographyExercise(capitals[capitalIndex]);
        }

    }
}
