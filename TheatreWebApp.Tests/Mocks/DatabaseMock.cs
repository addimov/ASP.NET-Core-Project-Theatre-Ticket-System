using System;
using Microsoft.EntityFrameworkCore;
using TheatreWebApp.Data;

namespace TheatreWebApp.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static TheatreDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<TheatreDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new TheatreDbContext(dbContextOptions);
            }
        }
    }
}
