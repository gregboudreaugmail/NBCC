using NBCC.Courses.DataAccess;
using NBCC.Courses.Models;
using NBCC.Courses.Queries;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.QueryHandlers;

public sealed class CoursesQueryHandler : IQueryHandler<CoursesQuery, IEnumerable<Course>>
{
    ICourseRepository CourseRepository { get; }

    public CoursesQueryHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));

    public async Task<IEnumerable<Course>> Handle(CoursesQuery query) => await CourseRepository.Get();
}