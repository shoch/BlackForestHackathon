using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe.exercise
{
    public class Wrapper
    {

        private string type { get; }

        private IExercise exercise { get; }

        public Wrapper(IExercise exercise)
        {
            this.exercise = exercise;
        }
    }
}
