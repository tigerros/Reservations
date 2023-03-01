namespace Reservations.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class ReservationDetailsViewModel : BaseViewModel {
	public Reservation Reservation { get; set; }
	[Required] public Guid BuildingId { get; set; }
	[Required] public Guid RoomId { get; set; }
	[Required] public Guid ReservationId { get; set; }
	public string? EditedName { get; set; }
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
	[DataType(DataType.Date)]
	public DateTime EditedStartTime { get; set; }
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
	[DataType(DataType.Date)]
	public DateTime EditedEndTime { get; set; }
}