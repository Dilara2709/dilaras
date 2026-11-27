using backend.Dtos;
using backend.Models;

namespace backend.Services;

public interface ITripService
{
    List<Trip> GetAll();
    Trip? GetById(int id);
    Trip Create(CreateTripRequest request, int userId);
    Trip? Assign(int id, int assigneeUserId);
    Trip? ChangeStatus(int id, string status);
}