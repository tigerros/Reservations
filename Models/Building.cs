namespace Reservations.Models;

public sealed class Building {
	public Guid Id { get; } = Guid.NewGuid();
	public string Name { get; set; }
	public List<Room> Rooms { get; set; }

	public Building(string name, List<Room>? rooms = null) {
		Name = name;
		Rooms = rooms ?? new List<Room>();
	}
}