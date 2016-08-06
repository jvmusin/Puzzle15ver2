using System.Collections.Generic;
using Ninject;

namespace Puzzle15.GameField.Tests
{
	public partial class GameField_Should
	{
		private static readonly IKernel Kernel = new StandardKernel(new GameFieldFactoriesNinjectModule());

		private static readonly IEnumerable<IGameFieldFactory<int>> GameFieldFactories =
			Kernel.GetAll<IGameFieldFactory<int>>();
	}
}