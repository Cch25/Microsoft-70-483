using System;
using System.Linq.Expressions;

namespace TypesWithReflection
{
    public class ExpressionTrees
    {
        public void ExpressionTreeMultiply()
        {
            ExpressionTreeExample ete = new ExpressionTreeExample();
            Expression<Func<int, int>> eTree = ete.BuildExpressionTrees<int>();
            Func<int, int> compileTree = eTree.Compile();
            Console.WriteLine(compileTree(5));
        }

        public void ExpressionTreeModifyToAdd()
        {
            ExpressionTreeExample ete = new ExpressionTreeExample();
            Func<int, int> add = ModifyExpressionTree(ete.BuildExpressionTrees<int>());
            Console.WriteLine(add(5));
        }

        public void IsAdultFemale()
        {
            Person p = new Person { Age = 19, Gender = "female", Hot = true };
            ExpressionTreeExample ete = new ExpressionTreeExample();
            Console.WriteLine(ete.IsAdultFemale(p));
        }

        private Func<T, T> ModifyExpressionTree<T>(Expression<Func<T, T>> eTree) where T : struct
        {
            ModifyToAdd modify = new ModifyToAdd();
            Expression<Func<T, T>> addExpression = (Expression<Func<T, T>>)modify.Modify(eTree);
            Func<T, T> addTo = addExpression.Compile();
            return addTo;
        }
    }

    public class ExpressionTreeExample
    {
        public Expression<Func<T, T>> BuildExpressionTrees<T>() where T : struct
        {
            // build the expression tree:
            // Expression<Func<int,int>> square = x => x * x;

            // The parameter for the expression is an integer
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");

            // The opertion to be performed is to square the parameter
            BinaryExpression binaryExpression = Expression.Multiply(parameterExpression, parameterExpression);

            // This creates an expression tree that describes the square operation
            return Expression.Lambda<Func<T, T>>(binaryExpression, new ParameterExpression[] { parameterExpression });
        }

        public bool IsAdultFemale(Person person)
        {
            //Expression<Func<Person, bool>> = p => ((p.Age >=18 && p.Gender.Equals("female")) || p.Hot == true)
            ParameterExpression pe = Expression.Parameter(typeof(Person), "p");
            MemberExpression meAge = Expression.Property(pe, "Age");
            MemberExpression meGender = Expression.Property(pe, "Gender");
            MemberExpression meHot = Expression.Property(pe, "Hot");
            ConstantExpression ceAge = Expression.Constant(18, typeof(int));
            ConstantExpression ceGender = Expression.Constant("female", typeof(string));
            ConstantExpression ceHotness = Expression.Constant(true, typeof(bool));
            Expression exHot = Expression.And(meHot, ceHotness);
            Expression exAgeGender = Expression.AndAlso(Expression.GreaterThanOrEqual(meAge, ceAge), Expression.Equal(meGender, ceGender));
            BinaryExpression be = Expression.Or(exHot, exAgeGender);
            Expression<Func<Person, bool>> expression = Expression.Lambda<Func<Person, bool>>(be, new ParameterExpression[] { pe });
            Func<Person, bool> func = expression.Compile();
            return func(person);
        }
    }
    public class ModifyToAdd : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Multiply)
            {
                Expression left = Visit(node.Left);
                Expression right = Visit(node.Right);
                // Make this binary expression an Add rather than a multiply operation.
                return Expression.Add(left, right);
            }
            return base.VisitBinary(node);
        }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool Hot { get; set; }
    }
}
