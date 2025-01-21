using System;
using Unity.Properties;

namespace GameName.Helpers
{
	public class BindableProperty<T>
	{
		private readonly Func<T> _getter;

		public BindableProperty(Func<T> getter)
		{
			_getter = getter;
		}

		[CreateProperty]
		public T Value => _getter();

		public static BindableProperty<T> Bind(Func<T> getter) => new BindableProperty<T>(getter);
	}
}