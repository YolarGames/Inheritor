using System.Collections.Generic;
using System.Linq;

namespace SkillSystemPrototype
{
	public class StatFloat
	{
		private readonly float _baseValue;
		private readonly List<float> _flatModifiers = new();
		private readonly List<float> _increaseModifiers = new();
		private readonly List<float> _moreModifiers = new();

		private float _flatValue;
		private float _increasedValue;
		private float _moreValue;

		public float BaseValue => _baseValue;
		public float FinalValue => _baseValue + _flatValue + _increasedValue + _moreValue;

		public StatFloat(float baseValue) =>
			_baseValue = baseValue;

		public void AddFlat(float value)
		{
			_flatModifiers.Add(value);
			CalculateFlat();
			CalculateIncrease();
			CalculateMore();
		}

		public void RemoveFlat(float value)
		{
			_flatModifiers.Remove(value);
			CalculateFlat();
			CalculateIncrease();
			CalculateMore();
		}

		public void AddIncrease(float value)
		{
			_increaseModifiers.Add(value);
			CalculateIncrease();
			CalculateMore();
		}

		public void RemoveIncrease(float value)
		{
			_increaseModifiers.Remove(value);
			CalculateIncrease();
			CalculateMore();
		}

		public void AddMore(float value)
		{
			_moreModifiers.Add(value);
			CalculateMore();
		}

		public void RemoveMore(float value)
		{
			_moreModifiers.Remove(value);
			CalculateMore();
		}

		private void CalculateFlat() =>
			_flatValue = _flatModifiers.Sum();

		private void CalculateIncrease() =>
			_increasedValue = (_baseValue + _flatValue) * _increaseModifiers.Sum();

		private void CalculateMore() =>
			_moreValue = (_baseValue + _flatValue + _increasedValue) * _moreModifiers.Sum();
	}
}