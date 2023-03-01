namespace Reservations.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using ViewModels;
using Services;

public sealed class HomeController : Controller {
	public IActionResult Index() => View(new BaseViewModel());

	public IActionResult Buildings(Guid? buildingToRemoveId = null) {
		if (buildingToRemoveId == null) return View(new BuildingsViewModel { Buildings = BuildingsService.GetBuildings(), });
		if (BuildingsService.GetBuilding(buildingToRemoveId.Value) == null) return View("Error", new ErrorViewModel { ErrorMessage = "Building not found.", });

		BuildingsService.RemoveBuilding(buildingToRemoveId.Value);

		return View(new BuildingsViewModel { Buildings = BuildingsService.GetBuildings(), });
	}

	[HttpPost]
	public IActionResult Buildings(BuildingsViewModel formModel) {
		BuildingsService.AddBuilding(new Building(formModel.NewName));
		
		return Buildings();
	}

	public IActionResult BuildingDetails(Guid id, Guid? roomToRemoveId = null) {
		Building? building = BuildingsService.GetBuilding(id);
		
		if (building == null) return View("Error", new ErrorViewModel { ErrorMessage = "Building not found.", });
		if (roomToRemoveId != null && !RoomsService.RemoveRoom(id, roomToRemoveId.Value)) return View("Error", new ErrorViewModel { ErrorMessage = "Room not found.", });

		return View(new BuildingDetailsViewModel { Building = building, });
	}

	[HttpPost]
	public IActionResult BuildingDetails(BuildingDetailsViewModel formModel) {
		Building? building;
		
		if (!string.IsNullOrEmpty(formModel.NewRoomName) && string.IsNullOrEmpty(formModel.EditedBuildingName)) {
			building = BuildingsService.GetBuilding(formModel.Id);

			if (building == null) return View("Error", new ErrorViewModel { ErrorMessage = "Building not found.", });
			
			Room room = new(formModel.Id, formModel.NewRoomName);
			RoomsService.AddRoom(room);

			return View(new BuildingDetailsViewModel { Building = building, }); 
		}
		
		building = BuildingsService.EditBuilding(formModel.Id, formModel.EditedBuildingName!);

		if (building == null) return View("Error", new ErrorViewModel { ErrorMessage = "Building not found.", });

		return View(new BuildingDetailsViewModel { Building = building, });
	}
	
	public IActionResult RoomDetails(Guid buildingId, Guid roomId, Guid? reservationToRemove = null) {
		Room? room = RoomsService.GetRoom(buildingId, roomId);

		if (room == null) return View("Error", new ErrorViewModel { ErrorMessage = "Room not found.", });
		if (reservationToRemove != null) ReservationsService.RemoveReservation(room, reservationToRemove.Value);

		return View(new RoomDetailsViewModel { Room = room, });
	}

	[HttpPost]
	public IActionResult RoomDetails(RoomDetailsViewModel formModel) {
		Room? room = RoomsService.GetRoom(formModel.BuildingId, formModel.RoomId);
		
		if (room == null) return View("Error", new ErrorViewModel { ErrorMessage = "Room not found.", });

		// Adding reservation
		if (!formModel.IsEditRoomForm) {
			if (string.IsNullOrWhiteSpace(formModel.NewReservationName))
				return View("Error",
					new ErrorViewModel {
						ErrorMessage = "Reservation name must not be null contain characters other than whitespace.",
					});

			bool wasAdded = ReservationsService.AddReservation(
				new Reservation(formModel.BuildingId,
					formModel.RoomId,
					formModel.NewReservationName,
					formModel.NewReservationStartTime,
					formModel.NewReservationEndTime)
				);

			if (!wasAdded) {
				return View("Error",
					new ErrorViewModel {
						ErrorMessage = "The reservation time collides with another reservation, or the building/room ID were not valid.",
					});
			}
			
			return View("RoomDetails", new RoomDetailsViewModel { Room = room, });
		}

		if (string.IsNullOrWhiteSpace(formModel.EditedRoomName)) {
			return View("Error",
				new ErrorViewModel {
					ErrorMessage = "Room name must not be null and contain characters other than whitespace.",
				});
		}

		room = RoomsService.EditRoom(formModel.BuildingId, formModel.RoomId, formModel.EditedRoomName);

		if (room == null) return View("Error", new ErrorViewModel { ErrorMessage = "Room not found.", });

		return View("RoomDetails", new RoomDetailsViewModel { Room = room, });
	}

	public IActionResult ReservationDetails(Guid buildingId, Guid roomId, Guid reservationId) {
		Reservation? reservation = ReservationsService.GetReservation(buildingId, roomId, reservationId);
	
		return reservation == null ?
			View("Error", new ErrorViewModel { ErrorMessage = "Reservation not found.", }) :
			View(new ReservationDetailsViewModel { Reservation = reservation, });
	}

	[HttpPost]
	public IActionResult ReservationDetails(ReservationDetailsViewModel formModel) {
		if (string.IsNullOrWhiteSpace(formModel.EditedName)) {
			return View("Error",
				new ErrorViewModel {
					ErrorMessage = "Reservation name must not be null contain characters other than whitespace.",
				});
		}
		
		Reservation? reservation = ReservationsService.EditReservation(formModel.BuildingId, formModel.RoomId, formModel.ReservationId, formModel.EditedName, formModel.EditedStartTime, formModel.EditedEndTime);

		if (reservation == null) return View("Error", new ErrorViewModel { ErrorMessage = "Reservation not found.", });

		return View(new ReservationDetailsViewModel { Reservation = reservation, });
	}
}