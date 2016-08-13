using Ninject.Modules;
using Puzzle15.Game;
using Puzzle15.GameField;
using Puzzle15.GameField.Immutable;

namespace Puzzle15.GraphicUI.Modules
{
	public class ClassicGameNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IGameFieldFactory<>)).To(typeof(ImmutableGameFieldFactory<>));
			Bind(typeof(IGameFieldShuffler<>)).To(typeof(GameFieldShuffler<>));
			Bind<IGameFactory<int>>().To<ClassicGameFactory>();
		}
	}
}