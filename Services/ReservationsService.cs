namespace Reservations.Services;

using Models;

/// <summary>
/// This service will only work if the <see cref="BuildingsService"/> and <see cref="RoomsService"/> are initialized.
/// </summary>
public static class ReservationsService {
	/// <summary>
	/// Returns all the reservations in the database.
	/// </summary>
	public static List<Reservation> GetReservations() {
		List<Building> buildings = BuildingsService.GetBuildings();
		List<Room> rooms = RoomsService.GetRooms();
		List<Reservation> reservations = new();

		foreach (Room room in rooms) { 
			reservations.AddRange(room.Reservations);
		}

		return reservations;
	}
	
	/// <summary>
	/// Returns all the reservations in the database.
	/// </summary>
	public static IEnumerable<Reservation> GetReservationsEnumerable() {
		List<Building> buildings = BuildingsService.GetBuildings();
		List<Room> rooms = RoomsService.GetRooms();

		foreach (Building building in buildings) {
			foreach (Room room in building.Rooms) {
				foreach (Reservation reservation in room.Reservations) {
					yield return reservation;
				}
			}
		}
	}

	/// <summary>
	/// Searches for a reservation with the specified ID.
	/// Returns null if no building/room/reservation with that ID exists.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room the reservation is for.</param>
	/// <param name="reservationId">The ID of the reservation to search for.</param>
	public static Reservation? GetReservation(Guid buildingId, Guid roomId, Guid reservationId) {
		Building? building = BuildingsService.GetBuilding(buildingId);

		if (building == null) return null;

		Room? room = RoomsService.GetRoom(buildingId, roomId);

		if (room == null) return null;

		foreach (Reservation reservation in room.Reservations) {
			if (reservation.Id != reservationId) continue;
			
			return reservation;
		}

		return null;
	}

	/// <summary>
	/// Returns the upcoming reservation.
	/// Returns <c>null</c> if there are no upcoming reservations (either no reservations overall, or all the reservations have expired).
	/// </summary>
	public static Reservation? UpcomingReservation() {
		DateTime upcomingTime = DateTime.MaxValue;
		Reservation? upcomingReservation = null;
		
		foreach (Reservation reservation in GetReservationsEnumerable()) {
			if (reservation.StartTime < upcomingTime && reservation.EndTime >= DateTime.Now) upcomingReservation = reservation;
		}

		return upcomingReservation;
	}

	/// <summary>
	/// Adds a reservation to the database.
	/// </summary>
	/// <returns>Returns <c>true</c> if the reservation was added (the building & room ID are valid and the reservation does not collide with another reservation); otherwise, <c>false</c>.</returns>
	public static bool AddReservation(Reservation reservation) {
		Building? building = BuildingsService.GetBuilding(reservation.BuildingId);
		
		if (building == null) return false;

		Room? room = RoomsService.GetRoom(reservation.BuildingId, reservation.RoomId);

		if (room == null) return false;

		foreach (Reservation otherReservation in room.Reservations) {
			if ((otherReservation.StartTime <= reservation.EndTime && otherReservation.EndTime >= reservation.EndTime) ||
			    (otherReservation.EndTime >= reservation.StartTime && otherReservation.StartTime <= reservation.StartTime)) {
				return false;
			}
		}

		room.Reservations.Add(reservation);
		BuildingsService.UpdateFile();

		return true;
	}
	
	/// <summary>
	/// Edits the room with the specified ID.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room the reservation is for.</param>
	/// <param name="reservationId">The ID of the reservation to edit.</param>
	/// <param name="newName">The new name of the room.</param>
	/// <returns>Returns the edited reservation if the reservation & building & room exist; otherwise, <c>null</c>.</returns>
	public static Reservation? EditReservation(Guid buildingId, Guid roomId, Guid reservationId, string? newName = null, DateTime? newStartTime = null, DateTime? newEndTime = null) {
		Reservation? reservation = GetReservation(buildingId, roomId, reservationId);

		if (reservation == null) return null;

		reservation.Name = newName ?? reservation.Name;
		reservation.StartTime = newStartTime ?? reservation.StartTime;
		reservation.EndTime = newEndTime ?? reservation.EndTime;
		
		BuildingsService.UpdateFile();

		return reservation;
	}

	/// <summary>
	/// Removes the reservation with the specified ID.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room the reservation is for.</param>
	/// <param name="reservationId">The ID of the reservation to remove.</param>
	/// <returns>Returns <c>true</c> if the reservation was removed; otherwise, <c>false</c>.</returns>
	public static bool RemoveReservation(Guid buildingId, Guid roomId, Guid reservationId) {
		Building? building = BuildingsService.GetBuilding(buildingId);

		if (building == null) return false;

		Room? room = RoomsService.GetRoom(buildingId, roomId);

		if (room == null) return false;

		for (int i = 0; i < room.Reservations.Count; i++) {
			Reservation reservation = room.Reservations[i];

			if (reservation.Id != reservationId) continue;

			room.Reservations.RemoveAt(i);
			BuildingsService.UpdateFile();
			return true;
		}

		return false;
	}

	/// <summary>
	/// Removes the reservation with the specified ID.
	/// </summary>
	/// <param name="room">The room the reservation is for.</param>
	/// <param name="reservationId">The ID of the reservation to remove.</param>
	/// <returns>Returns <c>true</c> if the reservation was removed; otherwise, <c>false</c>.</returns>
	public static bool RemoveReservation(Room room, Guid reservationId) {
		for (int i = 0; i < room.Reservations.Count; i++) {
			Reservation reservation = room.Reservations[i];

			if (reservation.Id != reservationId) continue;

			room.Reservations.RemoveAt(i);
			BuildingsService.UpdateFile();
			return true;
		}

		return false;
	}
}