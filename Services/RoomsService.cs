namespace Reservations.Services;

using Models;

/// <summary>
/// This service will only work if the <see cref="BuildingsService"/> is initialized.
/// </summary>
public static class RoomsService {
	/// <summary>
	/// Returns all the rooms in the database.
	/// </summary>
	public static List<Room> GetRooms() {
		List<Building> buildings = BuildingsService.GetBuildings();
		List<Room> rooms = new();

		foreach (Building building in buildings) {
			foreach (Room room in building.Rooms) rooms.Add(room);
		}

		return rooms;
	}
	
	/// <summary>
	/// Searches for a room with the specified ID.
	/// Returns null if no building/room with that ID exists.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room to search for.</param>
	public static Room? GetRoom(Guid buildingId, Guid roomId) {
		Building? building = BuildingsService.GetBuilding(buildingId);

		if (building == null) return null;

		foreach (Room room in building.Rooms) {
			if (room.Id == roomId) return room;
		}

		return null;
	}

	/// <summary>
	/// Adds a room to the database.
	/// </summary>
	public static void AddRoom(Room room) {
		Building? building = BuildingsService.GetBuilding(room.BuildingId);
		
		if (building == null) return;

		building.Rooms.Add(room);
		BuildingsService.UpdateFile();
	}
	
	/// <summary>
	/// Edits the room with the specified ID.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room to edit.</param>
	/// <param name="newName">The new name of the room.</param>
	/// <returns>Returns the edited room if the room & building exist; otherwise, <c>null</c>.</returns>
	public static Room? EditRoom(Guid buildingId, Guid roomId, string newName) {
		Room? room = GetRoom(buildingId, roomId);

		if (room == null) return null;

		room.Name = newName;
		
		BuildingsService.UpdateFile();

		return room;
	}
	
	/// <summary>
	/// Edits the room with the specified ID.
	/// </summary>
	/// <param name="room">The room to edit</param>
	/// <param name="newName">The new name of the room.</param>
	/// <returns>Returns the edited room if the room and it's building exist; otherwise, <c>null</c>.</returns>
	public static Room? EditRoom(Room? room, string newName) {
		if (room == null || BuildingsService.GetBuilding(room.BuildingId) == null) return null;

		room.Name = newName;
		BuildingsService.UpdateFile();

		return room;
	}

	/// <summary>
	/// Removes the room with the specified ID.
	/// </summary>
	/// <param name="buildingId">The ID of the building the room is in.</param>
	/// <param name="roomId">The ID of the room to remove.</param>
	/// <returns>Returns <c>true</c> if the room was removed; otherwise, <c>false</c>.</returns>
	public static bool RemoveRoom(Guid buildingId, Guid roomId) {
		Building? building = BuildingsService.GetBuilding(buildingId);

		if (building == null) return false;

		for (int i = 0; i < building.Rooms.Count; i++) {
			Room room = building.Rooms[i];

			if (room.Id != roomId) continue;

			building.Rooms.RemoveAt(i);
			BuildingsService.UpdateFile();
			return true;
		}

		return false;
	}
}