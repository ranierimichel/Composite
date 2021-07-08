using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeSpecification
{
    public enum Color
    {
        Red,
        Blue,
        Green
    }
    public class Product
    {
        private Color color;

        public Color Color { get => color; set => color = value; }

        public override string ToString()
        {
            return color.ToString();
        }

    }

    public abstract class Specification<T>
    {
        public abstract bool IsSatisfied(T p);

        public static Specification<T> operator &(
            Specification<T> first, Specification<T> second)
        {
            return new AndSpecification<T>(first, second);
        } 
        public static Specification<T> operator |(
            Specification<T> first, Specification<T> second)
        {
            return new OrSpecification<T>(first, second);
        }
    }
    public class ColorSpecification : Specification<Product>
    {
        private readonly Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public override bool IsSatisfied(Product p)
        {
            return p.Color == color;
        }
    }

    public abstract class CompositeSpecification<T> : Specification<T>
    {
        protected readonly Specification<T>[] Items;
        public CompositeSpecification(params Specification<T>[] items)
        {
            Items = items;
        }
    }

    // combinator
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params Specification<T>[] items) : base(items)
        {
        }

        public override bool IsSatisfied(T t)
        {
            // Any -> OrSpecification
            return Items.All(i => i.IsSatisfied(t));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params Specification<T>[] items) : base(items)
        {            
        }

        public override bool IsSatisfied(T t)
        {
            // All -> AndSpecification
            return Items.Any(i => i.IsSatisfied(t));
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Product one = new Product
            {
                Color = Color.Blue
            };
            Product two = new Product
            {
                Color = Color.Red
            };

            ColorSpecification color = new ColorSpecification(Color.Blue);
            ColorSpecification color1 = new ColorSpecification(Color.Green);
            ColorSpecification color2 = new ColorSpecification(Color.Red);
            
            AndSpecification<Product> and = new AndSpecification<Product>(color,color1);
            OrSpecification<Product> or = new OrSpecification<Product>(color,color1,color2);
                                  
            Console.WriteLine($"Product one color: {one}");
            Console.WriteLine($"Product two color: {two}");
            Console.WriteLine(and.IsSatisfied(one));
            Console.WriteLine(or.IsSatisfied(two));


        }
    }
}
