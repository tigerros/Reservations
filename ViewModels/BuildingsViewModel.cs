namespace Reservations.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class BuildingsViewModel : BaseViewModel {
	public List<Building> Buildings { get; set; }
	[DataType(DataType.Text)]
	[Required]
	public string NewName { get; set; }
}