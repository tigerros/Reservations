namespace Reservations.Models;

using System.ComponentModel.DataAnnotations;

public sealed class Reservation {
	public Guid BuildingId { get; set; }
	public Guid RoomId { get; set; }
	public Guid Id { get; } = Guid.NewGuid();
	public string Name { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }

	public Reservation(Guid buildingId, Guid roomId, string name, DateTime startTime, DateTime endTime) {
		BuildingId = buildingId;
		RoomId = roomId;
		Name = name;
		StartTime = startTime;
		EndTime = endTime;
	}
}