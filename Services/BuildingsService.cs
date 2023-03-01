namespace Reservations.Services;

using System.Text.Json;
using Models;

/// <summary>
/// This is the "top-level" service that the <see cref="RoomsService"/> and <see cref="ReservationsService"/> depend on.
/// </summary>
public static class BuildingsService {
	private static List<Building> _buildings = new();
	private static string _jsonFilePath = "";

	/// <summary>
	/// Initiates the service according to the value of the <paramref name="jsonFilePath"/> paramater.
	/// </summary>
	/// <exception cref="Exception">Thrown if the JSON file is not in the correct format.</exception>
	public static void Initiate(string jsonFilePath) {
		_jsonFilePath = jsonFilePath;

		string jsonText = File.ReadAllText(_jsonFilePath);

		if (jsonText == "") return;

		_buildings = JsonSerializer.Deserialize<List<Building>>(jsonText) ?? throw new Exception("The JSON file is not in the correct format.");
		TryFixIds();
	}

	/// <summary>
	/// Updates the JSON file according to the buildings list.
	/// </summary>
	public static void UpdateFile() {
		File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(_buildings));
	}

	/// <summary>
	/// A utility method to be used in case something goes wrong with the IDs.
	/// It can be a simple problem of a room or reservation having a different parent ID than their parent actually has.
	/// </summary>
	public static void TryFixIds() {
		foreach (Building building in _buildings) {
			foreach (Room room in building.Rooms) {
				room.BuildingId = building.Id;

				foreach (Reservation reservation in room.Reservations) {
					reservation.BuildingId = building.Id;
					reservation.RoomId = room.Id;
				}
			}
		}
	}

	/// <summary>
	/// Returns all the buildings in the database.
	/// </summary>
	public static List<Building> GetBuildings() => _buildings;
	
	/// <summary>
	/// Searches for a building with the specified ID.
	/// Returns null if no building with that ID exists.
	/// </summary>
	/// <param name="id">The ID of the building to search for.</param>
	public static Building? GetBuilding(Guid id) {
		foreach (Building building in _buildings) {
			if (building.Id == id) return building;
		}

		return null;
	}

	/// <summary>
	/// Adds a building to the database.
	/// </summary>
	/// <param name="building">The building to add.</param>
	public static void AddBuilding(Building building) {
		_buildings.Add(building);
		UpdateFile();
	}

	/// <summary>
	/// Edits the building with the specified ID.
	/// </summary>
	/// <param name="id">The ID of the building to edit.</param>
	/// <param name="newName">The new name of the building.</param>
	/// <returns>Returns the edited building if the building exists; otherwise, <c>null</c>.</returns>
	public static Building? EditBuilding(Guid id, string newName) {
		Building? building = GetBuilding(id);

		if (building == null) return null;

		building.Name = newName;
		
		UpdateFile();

		return building;
	}

	/// <summary>
	/// Removes the building with the specified ID.
	/// </summary>
	/// <param name="id">The ID of the building to remove.</param>
	/// <returns>Returns <c>true</c> if the building was removed; otherwise, <c>false</c>.</returns>
	public static bool RemoveBuilding(Guid id) {
		for (int i = 0; i < _buildings.Count; i++) {
			Building building = _buildings[i];

			if (building.Id != id) continue;

			_buildings.RemoveAt(i);
			UpdateFile();
			
			return true;
		}

		return false;
	}
}