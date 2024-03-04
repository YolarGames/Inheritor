using System;
using UnityEngine;

namespace SkillSystemPrototype
{
	public static class PlayerInputEvents
	{
		public static Action<Vector2> OnMove = vector2 => { };
		public static Action OnMouseDown0 = () => { };
		public static Action<Vector2> OnMouseHold0 = vector2 => { };
		public static Action OnMouseUp0 = () => { };
		public static Action OnBackPressed = () => { };
	}
}