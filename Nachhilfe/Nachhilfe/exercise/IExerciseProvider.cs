using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public interface IExerciseProvider<IExercise>
    {
        IExercise NextExercise();
    }
}
