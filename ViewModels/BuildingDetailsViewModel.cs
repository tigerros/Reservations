namespace Reservations.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class BuildingDetailsViewModel : BaseViewModel {
	public Building Building { get; set; }
	[Required] public Guid Id { get; set; }
	public string? EditedBuildingName { get; set; }
	public string? NewRoomName { get; set; }
}