using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Xp.Common.Resources;
using Xp.Domain;

namespace Xp.Infrastructure
{
	public abstract class UnitOfWork<T> : IUnitOfWork<T>
		where T : IContext
	{
		private IContext Context;
        private TransactionScope Scope;

        #region Constructors
        public UnitOfWork(IContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			Context = context;
		}
		#endregion Constructors

		#region SaveAsync
		public async Task SaveAsync()
		{
			try
			{
				if (Context != null)
					await Context.SaveAsync();
			}
			catch (DbUpdateException ex)
			{
				FilterException(ex);
			}
			catch (UpdateException ex)
			{
				FilterException(ex);
			}
			catch (SqlException ex)
			{
				FilterException(ex);
			}
			catch (DbEntityValidationException ex)
			{
				FilterException(ex);
			}
		}
		#endregion SaveAsync

		#region Dispose
		public void Dispose()
		{
			if (Context != null)
				Context.Dispose();
			Context = null;
		}
		#endregion Dispose

		#region FilterException
		private void FilterException(DbUpdateException exception)
		{
			if (exception == null)
				return;

			FilterException(exception.InnerException as UpdateException);

			throw exception;
		}
		private void FilterException(UpdateException exception)
		{
			if (exception == null)
				return;

			FilterException(exception.InnerException as SqlException);

			throw exception;
		}
		private void FilterException(SqlException exception)
		{
			if (exception == null)
				return;

			// The DELETE statement conflicted with the REFERENCE constraint "FK_dbo.ControlFormAssignTemplates_dbo.ControlTemplateChecks_CheckId". The conflict occurred in database "payintest", table "dbo.ControlFormAssignTemplates", column 'CheckId'.
			var result = Regex.Match(
				exception.Message,
				".*Instrucción DELETE en conflicto con la restricción REFERENCE \".*\\.(.*)Id\". El conflicto ha aparecido en la base de datos \".*\", tabla \"dbo\\.(.*)\", column '.*'.*",
				RegexOptions.Multiline
			);
			if (result.Success)
				throw new ApplicationException(GlobalResources.ExceptionForeignKey.FormatString(result.Groups[1].Value, result.Groups[2].Value), exception);

			result = Regex.Match(
				exception.Message,
				".*The DELETE statement conflicted with the REFERENCE constraint \".*\\.(.*)Id\". The conflict occurred in database \".*\", table \"dbo\\.(.*)\", column '.*'.*",
				RegexOptions.Multiline
			);
			if (result.Success)
				throw new ApplicationException(GlobalResources.ExceptionForeignKey.FormatString(result.Groups[1].Value, result.Groups[2].Value), exception);

			throw exception;
		}
		private void FilterException(DbEntityValidationException exception)
		{
			if (exception == null)
				return;

			var exceptions = new List<Exception>();
			foreach (var entityError in exception.EntityValidationErrors)
			{
				foreach (var validationError in entityError.ValidationErrors)
					exceptions.Add(new ArgumentException(
						"{0} ({1}): {2}".FormatString(
							entityError.Entry.Entity.GetType().Name,
							entityError.Entry.State,
							validationError.ErrorMessage
						), 
						validationError.PropertyName
					));
			}
			throw new AggregateException(exceptions);
		}
		#endregion FilterException

		#region BeginTransaction
		public void BeginTransaction()
		{
            Scope = new TransactionScope();
        }
		#endregion BeginTransaction

		#region CommitTransaction
		public void CommitTransaction()
		{
            Scope.Complete();
        }
		#endregion CommitTransaction

		#region RollbackTransaction
		public void RollbackTransaction()
		{
            Scope.Dispose();
		}
		#endregion RollbackTransaction
	}
}
