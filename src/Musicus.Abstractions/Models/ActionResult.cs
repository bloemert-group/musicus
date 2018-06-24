namespace Musicus.Abstractions.Models
{
	public class ActionResult<T> : IActionResult<T>
	{
		public bool Succeed { get; set; }
		public T Data { get; set; }
		public string ErrorMessage { get; set; }

		public static IActionResult<T> Error(string errorMessage)
		{
			return new ActionResult<T>
			{
				Succeed = false,
				ErrorMessage = errorMessage
			};
		}

		public static IActionResult<T> Success(T data)
		{
			return new ActionResult<T>
			{
				Succeed = true,
				Data = data
			};
		}
	}
}
