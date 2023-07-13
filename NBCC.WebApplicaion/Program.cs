/*
Note 1:
All web applications will begin with a Main function.  In modern framework examples, you'll
see instances like I have below where I setup a Startup file and some where they skip the
startup file although and use a minimalist style.  The code is interchangeable and offer
no performance change one way or the other.  I opt for the startup file as it's more recognizable 
C# code but I wouldn't be argue against the minimalist approach either.

What this class offers is some setup.  As it reads, the host (framework host that IIS will read) will
make a builder, set configuration, build and run the product.  This will only happen once in the 
cycle of the product, as it's turned on. 

You may expand this section by adding logging configuration, appsettings changes or pretty much
anything that can go in the startup file.  To keep things clean and consistent, I rarely make
changes here and delegate the work to startup.

*/
namespace NBCC.Courses.WebApplication;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(static webBuilder => webBuilder.UseStartup<Startup>())
            .Build().Run();
}