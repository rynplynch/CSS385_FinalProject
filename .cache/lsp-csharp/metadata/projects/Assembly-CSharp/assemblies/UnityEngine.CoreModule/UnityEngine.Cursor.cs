using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine;

/// <summary>
///   <para>Cursor API for setting the cursor (mouse pointer).</para>
/// </summary>
[NativeHeader("Runtime/Export/Input/Cursor.bindings.h")]
public class Cursor
{
	/// <summary>
	///   <para>Determines whether the hardware pointer is visible or not.</para>
	/// </summary>
	public static extern bool visible
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	/// <summary>
	///   <para>Determines whether the hardware pointer is locked to the center of the view, constrained to the window, or not constrained at all.</para>
	/// </summary>
	public static extern CursorLockMode lockState
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	private static void SetCursor(Texture2D texture, CursorMode cursorMode)
	{
		SetCursor(texture, Vector2.zero, cursorMode);
	}

	/// <summary>
	///   <para>Sets a custom cursor to use as your cursor.</para>
	/// </summary>
	/// <param name="texture">The texture to use for the cursor. To use a texture, import it with `Read/Write` enabled. Alternatively, you can use the default cursor import setting. If you created your cursor texture from code, it must be in RGBA32 format, have alphaIsTransparency enabled, and have no mip chain. To use the default cursor, set the texture to `Null`.</param>
	/// <param name="hotspot">The offset from the top left of the texture to use as the target point. This must be in the bounds of the cursor.</param>
	/// <param name="cursorMode">Whether to render this cursor as a hardware cursor on supported platforms, or force software cursor.</param>
	public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
	{
		SetCursor_Injected(Object.MarshalledUnityObject.Marshal(texture), ref hotspot, cursorMode);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetCursor_Injected(IntPtr texture, [In] ref Vector2 hotspot, CursorMode cursorMode);
}
