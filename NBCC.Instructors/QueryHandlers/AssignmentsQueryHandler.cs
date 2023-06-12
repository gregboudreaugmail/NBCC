using NBCC.CQRS.Commands;
using NBCC.Instructors.DataAccess.Assignments;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;

namespace NBCC.Instructors.QueryHandlers;

public sealed class AssignmentsQueryHandler : IQueryHandler<AssignmentsQuery, IEnumerable<Assignment>>
{
    IAssignmentRepository AssignmentRepository { get; }

    public AssignmentsQueryHandler(IAssignmentRepository assignmentRepository) => AssignmentRepository = assignmentRepository;

    public async Task<IEnumerable<Assignment>> Handle(AssignmentsQuery query) => await AssignmentRepository.Get(query.InstructorId);
}