using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Nachhilfe;

namespace Testing
{
    [TestClass]
    public class MathSubstractionExerciseProviderTest
    {
        [TestMethod]
        public void TestNextExercise()
        {
            int permutation = 4;
            int min = 1;
            int max = 10;

            var p = new MathSubstractionExerciseProvider(new Random(100), permutation, min, max);

            var e = p.NextExercise();

            foreach (int number in e.numbers)
            {
                if(number < min && number > max)
                {
                    Assert.Fail();
                }
            } 

        }
    }
}
