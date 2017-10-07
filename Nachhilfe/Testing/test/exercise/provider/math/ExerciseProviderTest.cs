using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nachhilfe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    
    public class ExerciseProviderTest
    {

        public void TestMathExercise(IExerciseProvider<MathExercise> provider, int min, int max)
        {
            for (int i = 0; i < 100; i++)
            {
                var e = provider.NextExercise();

                foreach (int number in e.numbers)
                {
                    if (number < min || number > max)
                    {
                        Assert.Fail("falscher Werte generiert: " + number);
                    }
                }

                if (e.Solution() < min || e.Solution() > max)
                {
                    Assert.Fail("falsche Loesung generiert: " + e.Solution());
                }
            }
        }
    }
}
