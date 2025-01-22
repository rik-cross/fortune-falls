using System;

namespace Engine
{
	/// <summary>
	/// Defines the mouse buttons.
	/// </summary>
	[Flags]
	public enum MouseButtons
	{
		/// <summary>
		/// Left button.
		/// </summary>
		LeftMouseButton = 0x1,
		/// <summary>
		/// Right button.
		/// </summary>
		RightMouseButton = 0x2,
		/// <summary>
		/// Middle button.
		/// </summary>
		MiddleMouseButton = 0x4
	}
}
