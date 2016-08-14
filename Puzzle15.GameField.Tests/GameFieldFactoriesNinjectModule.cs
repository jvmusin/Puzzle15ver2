using Ninject.Modules;

namespace Puzzle15.GameField.Tests
{
	public class GameFieldFactoriesNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IGameFieldFactory<>)).To(typeof(ImmutableGameFieldFactory<>));
			Bind(typeof(IGameFieldFactory<>)).To(typeof(MutableGameFieldFactory<>));
			Bind(typeof(IGameFieldFactory<>)).To(typeof(WrappingGameFieldFactory<>));
		}
	}
}