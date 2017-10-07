using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Nachhilfe;

namespace Testing
{
    [TestClass]
    public class MathAdditionExerciseProviderTest : ExerciseProviderTest
    {
        [TestMethod]
        public void TestNextExercise()
        {
            int permutation = 4;
            int min = 1;
            int max = 10;
            var provider = new MathAdditionExerciseProvider(new Random(), permutation, min, max);
            base.TestMathExercise(provider, min, max);
        }
    }
}
