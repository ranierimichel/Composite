using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeExercise
{
    public interface IValueContainer : IEnumerable<int>
    {

    }
    public class SingleValue : IValueContainer
    {
        public int Value;

        public IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class ManyValues : List<IValueContainer>
    {
    }
    public static class ExtensionMethods
    {
        public static int Sum(this List<IValueContainer> containers)
        {
            int result = 0;
            foreach (var c in containers)
                foreach (var i in c)
                    result += i;
            return result;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            SingleValue valor1 = new SingleValue{Value = 1};
            SingleValue valor2 = new SingleValue{Value = 2};
            SingleValue valor3 = new SingleValue{Value = 3};

            ManyValues values = new ManyValues() { valor1, valor2, valor3};
            Console.WriteLine(ExtensionMethods.Sum(values));
        }
    }
}
