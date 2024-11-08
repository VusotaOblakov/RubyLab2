using DistributedDatabase.DAL;
using DistributedDatabase.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistributedDatabase_.API.Services
{
    public class DatabaseFillingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseFillingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DistributedDatabaseDbContext>();

            dbContext.Database.Migrate();

            if(!dbContext.Students.Any() && !dbContext.Courses.Any())  //if db is empty 
            {
                await Fill(dbContext); 
            }
        }

        protected async Task Fill(DistributedDatabaseDbContext dbContext)
        {
            var students = new List<Student>
            {
                new Student 
                { 
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Birthday=new DateOnly(2000,12,10),
                    Group = 501
                },
                new Student 
                { 
                    FirstName = "Tsiupa",
                    LastName = "Oleg",
                    Birthday=new DateOnly(2003,2,28),
                    Group = 507
                },
                new Student
                {
                    FirstName = "Katya",
                    LastName = "Svunarchuk",
                    Birthday=new DateOnly(2003,11,22),
                    Group = 505
                },
                new Student 
                {
                    FirstName = "Olena",
                    LastName = "Voloshcuk",
                    Birthday=new DateOnly(2002,1,1),
                    Group = 504
                },
                new Student 
                { 
                    FirstName = "Mychle",
                    LastName = "Tarasov",
                    Birthday=new DateOnly(2002,5,13),
                    Group = 503
                }
            };

            var courses = new List<Course>
            {
                new Course { Title = "OS Real Time", Credits = 6 },
                new Course { Title = "Organization of the database", Credits = 8 },
                new Course { Title = "Forecasting in system analysis", Credits = 8 }
            };

            await dbContext.Students.AddRangeAsync(students);
            await dbContext.Courses.AddRangeAsync(courses);
            await dbContext.SaveChangesAsync();

            var studentsCourses = new List<StudentCourse>
            {
                new StudentCourse { StudentId = students[0].StudentId, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[0].StudentId, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[0].StudentId, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[1].StudentId, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[1].StudentId, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[2].StudentId, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[2].StudentId, CourseId = courses[0].CourseId },
                new StudentCourse { StudentId = students[2].StudentId, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[3].StudentId, CourseId = courses[2].CourseId },
                new StudentCourse { StudentId = students[4].StudentId, CourseId = courses[1].CourseId },
                new StudentCourse { StudentId = students[4].StudentId, CourseId = courses[1].CourseId },
            };

            await dbContext.StudentCourses.AddRangeAsync(studentsCourses);
            await dbContext.SaveChangesAsync();
        }
    }
}
