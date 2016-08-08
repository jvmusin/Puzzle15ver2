using System.Collections.Generic;
using Ninject;

namespace Puzzle15.GameField.Tests
{
	public abstract class GameFieldTestsBase
	{
		private static readonly IKernel Kernel = new StandardKernel(new GameFieldFactoriesNinjectModule());

		protected static readonly IEnumerable<IGameFieldFactory<int>> GameFieldFactories =
			Kernel.GetAll<IGameFieldFactory<int>>();
	}
}