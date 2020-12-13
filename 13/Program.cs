using System;
using System.Linq;

namespace _13
{
    class Program
    {
        static void Main(string[] args)
        {
            int earliestDeparture = 1000340;
            string timeString = "13,x,x,x,x,x,x,37,x,x,x,x,x,401,x,x,x,x,x,x,x,x,x,x,x,x,x,17,x,x,x,x,19,x,x,x,23,x,x,x,x,x,29,x,613,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41";
            var validIds = timeString.Split(',').Where(s => s != "x").Select(int.Parse);

            var resultId = validIds.OrderBy(id => id - (earliestDeparture % id)).First();
            var timeToWait = resultId - (earliestDeparture % resultId);

            Console.WriteLine(resultId);
            Console.WriteLine(timeToWait);
            Console.WriteLine(resultId * timeToWait);
        }
    }
}
