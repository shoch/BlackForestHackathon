using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    interface IExercise<T>
    {
        string GetQuestion();
        string GetSolution();
        bool ValidateAnswer(T answer);
    }
}
