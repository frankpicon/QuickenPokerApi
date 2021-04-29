using System;
using System.Collections.Generic;
using System.Text;
using Quicken.Poker.Dal.Quicken.Poker.Models;

namespace Quicken.Poker.Core.Services
{

	public abstract class ServiceBase : IDisposable
	{
		private PokerGameContext dbContext = null;

		public PokerGameContext DbContext
		{
			get { return dbContext ?? (dbContext = new PokerGameContext()); }
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				dbContext?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

}
