using MVC.Abstractions.Services.AD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.Implementations.AD
{
    public class UserService : IUserService
    {
        public int[] GetNumbers()
        {
            int[] result = new int[10];

            Parallel.For(0, 10, i =>
            {
                result[i] = i;
            });

            return result;
        }
    }
}
