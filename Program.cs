using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Scratch3D.View;

namespace Scratch3D
{
	public static class Program
	{
		private static void Main()
		{

			var nativeWindowSettings = new NativeWindowSettings()
			{
				ClientSize = new Vector2i(800, 600),
				Title = "Create a Window"
			};

			using (var window = new SimpleWindow(GameWindowSettings.Default, nativeWindowSettings))
			{
				window.Run();
			}

		}
	}
}
