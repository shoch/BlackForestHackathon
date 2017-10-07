using System;
using System.Collections.Generic;
using System.Text;

namespace Nachhilfe
{
    public interface IExercise
    {
        string GetQuestion();
        string GetSolution();
        bool ValidateAnswer(string answer);
    }
}
