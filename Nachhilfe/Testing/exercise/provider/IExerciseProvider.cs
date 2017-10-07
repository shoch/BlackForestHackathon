using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    interface IExerciseProvider<IExercise>
    {
        IExercise NextExercise();
    }
}
