using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace Puzzle15.GameField.Tests
{
	public abstract class GameFieldTestsBase
	{
		private static readonly IKernel Kernel = new StandardKernel(new GameFieldFactoriesNinjectModule());

		protected static IEnumerable<IGameFieldFactory<int>> GameFieldFactories =>
			Kernel.GetAll<IGameFieldFactory<int>>();

		protected static IEnumerable<IGameFieldFactory<int>> ClassicGameFieldFactories =>
			GameFieldFactories.Where(f => !(f is WrappingGameFieldFactory<int>));
	}
}