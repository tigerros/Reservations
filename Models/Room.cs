namespace Reservations.Models; 

public sealed class Room {
	public Guid BuildingId { get; set; }
	public Guid Id { get; } = Guid.NewGuid();
	public string Name { get; set; }
	public List<Reservation> Reservations { get; set; }

	public Reservation? ActiveReservation {
		get {
			foreach (Reservation reservation in Reservations) {
				if (reservation.StartTime <= DateTime.Now) return reservation;
			}

			return null;
		}
	}

	public Room(Guid buildingId, string name, List<Reservation>? reservations = null) {
		BuildingId = buildingId;
		Name = name;
		Reservations = reservations ?? new List<Reservation>();
	}
}