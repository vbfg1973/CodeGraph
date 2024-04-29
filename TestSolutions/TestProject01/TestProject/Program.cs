using TestProject.Domain;

namespace TestProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ITest t = new Test();
            
            t.DoSomethingCool();
        }
    }
}