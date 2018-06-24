namespace Musicus.Abstractions.Models
{
	public interface IActionResult<T>
	{
		bool Succeed { get; set; }
		T Data { get; set; }
		string ErrorMessage { get; set; }
	}
}
