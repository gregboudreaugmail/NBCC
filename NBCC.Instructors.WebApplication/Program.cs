namespace NBCC.Instructors.WebApplication;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(static webBuilder => webBuilder.UseStartup<Startup>())
            .Build().Run();
}