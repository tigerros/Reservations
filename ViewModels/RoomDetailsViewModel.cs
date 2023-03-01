namespace Reservations.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class RoomDetailsViewModel : BaseViewModel {
	public Room Room { get; set; }
	[Required] public Guid BuildingId { get; set; }
	[Required] public Guid RoomId { get; set; }
	public bool IsEditRoomForm { get; set; }
	[MinLength(1)]
	public string? EditedRoomName { get; set; }
	public string? NewReservationName { get; set; }
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
	[DataType(DataType.Date)]
	public DateTime NewReservationStartTime { get; set; }
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
	[DataType(DataType.Date)]
	public DateTime NewReservationEndTime { get; set; }
}