namespace InheritorCode.SkillSystemPrototype
{
	public sealed class PlayerModel
	{
		private StatFloat _movementSpeed = new(3);
		public StatFloat MovementSpeed => _movementSpeed;
	}
}