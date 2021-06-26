using System;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace Xp.Infrastructure.Db
{
	public class MTMigrationSqlGenerator : SqlServerMigrationSqlGenerator
	{
		private readonly string Schema;

		#region Constructors
		public MTMigrationSqlGenerator(string schema)
		{
			Schema = schema;
		}
		#endregion Constructors

		#region Generate
		// Table
		protected override void Generate(CreateTableOperation operation)
		{
			var name = GetNameWithReplacedSchema(operation.Name);
			var newOperation = new CreateTableOperation(name, operation.AnonymousArguments);
			newOperation.PrimaryKey = operation.PrimaryKey;
			foreach (var column in operation.Columns)
				newOperation.Columns.Add(column);

			base.Generate(newOperation);
		}
		protected override void Generate(DropTableOperation operation)
		{
			var name = GetNameWithReplacedSchema(operation.Name);
			var newOperation = new CreateTableOperation(name, operation.AnonymousArguments);
			base.Generate(newOperation);
		}
		// Columns
		protected override void Generate(AddColumnOperation operation)
		{
			var name = GetNameWithReplacedSchema(operation.Table);
			var newOperation = new AddColumnOperation(name, operation.Column, operation.AnonymousArguments);
			base.Generate(newOperation);
		}
		protected override void Generate(AlterColumnOperation operation)
		{
			var name = GetNameWithReplacedSchema(operation.Table);
			var newOperation = new AlterColumnOperation(name, operation.Column, operation.IsDestructiveChange, operation.AnonymousArguments);
			base.Generate(newOperation);
		}
		protected override void Generate(DropColumnOperation operation)
		{
			var name = GetNameWithReplacedSchema(operation.Table);
			var newOperation = new DropColumnOperation(name, operation.Name, operation.AnonymousArguments);
			base.Generate(newOperation);
		}
		// Primary key
		protected override void Generate(AddPrimaryKeyOperation operation)
		{
			operation.Table = GetNameWithReplacedSchema(operation.Table);
			base.Generate(operation);
		}
		protected override void Generate(DropPrimaryKeyOperation operation)
		{
			operation.Table = GetNameWithReplacedSchema(operation.Table);
			base.Generate(operation);
		}
		// Foreign key
		protected override void Generate(AddForeignKeyOperation operation)
		{
			operation.DependentTable = GetNameWithReplacedSchema(operation.DependentTable);
			operation.PrincipalTable = GetNameWithReplacedSchema(operation.PrincipalTable);
			base.Generate(operation);
		}
		protected override void Generate(DropForeignKeyOperation operation)
		{
			operation.DependentTable = GetNameWithReplacedSchema(operation.DependentTable);
			operation.PrincipalTable = GetNameWithReplacedSchema(operation.PrincipalTable);
			base.Generate(operation);
		}
		// Index
		protected override void Generate(CreateIndexOperation operation)
		{
			operation.Table = GetNameWithReplacedSchema(operation.Table);
			base.Generate(operation);
		}
		protected override void Generate(DropIndexOperation operation)
		{
			operation.Table = GetNameWithReplacedSchema(operation.Table);
			base.Generate(operation);
		}
		#endregion Generate

		#region GetNameWithReplacedSchema
		private string GetNameWithReplacedSchema(string name)
		{
			string[] nameParts = name.Split('.');
			string newName;

			switch (nameParts.Length)
			{
				case 1:
					newName = string.Format("{0}.{1}", Schema, nameParts[0]);
					break;

				case 2:
					newName = string.Format("{0}.{1}", Schema, nameParts[1]);
					break;

				case 3:
					newName = string.Format("{0}.{1}.{2}", Schema, nameParts[1], nameParts[2]);
					break;

				default:
					throw new NotSupportedException();
			}

			return newName;
		}
		#endregion GetNameWithReplacedSchema
	}
}
