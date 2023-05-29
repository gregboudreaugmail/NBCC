using NBCC.Instructors.DataAccess;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;
using NBCC.CQRS.Commands;

namespace NBCC.Instructors.QueryHandlers;

public sealed class InstructorsQueryHandler : IQueryHandler<InstructorsQuery, IEnumerable<Instructor>>
{
    IInstructorRepository InstructorRepository { get; }

    public InstructorsQueryHandler(IInstructorRepository instructorRepository) => InstructorRepository = instructorRepository;

    public async Task<IEnumerable<Instructor>> Handle(InstructorsQuery query) => await InstructorRepository.Get();
}