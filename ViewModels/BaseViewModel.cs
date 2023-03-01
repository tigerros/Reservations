namespace Reservations.ViewModels; 

/// <summary>
/// Holds all the properties that every view has.
/// Basically a replacement for <c>ViewData</c>, but doesn't introduce magic strings or dynamic types.
/// </summary>
public class BaseViewModel {
	/// <summary>
	/// The title of the website.
	/// </summary>
	public string Title { get; set; } = "";
}