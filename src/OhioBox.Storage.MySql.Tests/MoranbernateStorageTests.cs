﻿using System;
using NUnit.Framework;
using OhioBox.Storage.MySql.Bootstrap;
using OhioBox.Storage.MySql.Moranbernate;
using OhioBox.Storage.MySql.Tests.Bootstrap;
using OhioBox.Storage.MySql.Tests.Mapping;

namespace OhioBox.Storage.MySql.Tests
{
	[TestFixture]
	public class MoranbernateStorageTests
	{
		private ISqlConnectionFactory _sqlConnectionFactory;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_sqlConnectionFactory = Bootstrapper.Init();
		}


		private IStorage<UserDto> _target;

		[SetUp]
		public void Setup()
		{
			_target = new MoranbernateStorage<UserDto>(_sqlConnectionFactory, new DummyLogger(), new DefaultMetricsReporter());
			ClearDatabase();
		}

		[TearDown]
		public void TearDown()
		{
			ClearDatabase();
		}

		private void ClearDatabase()
		{
			var all = _target.Query(q => q.IsNotNull(x => x.Id));
			_target.Delete(all);
		}

		[Test]
		public void Query_WhenCalled_ReturnsItemsByPredicate()
		{
			AddUser(1L, "Doron");
			AddUser(2L, "Shani");
			AddUser(3L, "Tomer");

			var result = _target.Query(q => q.Equal(x => x.Name, "Shani"));

			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result, Has.Some.Matches<UserDto>(u => u.Id == 2L && u.Name == "Shani"));
		}

		private void AddUser(long id, string name)
		{
			_target.Add(new UserDto { Id = id, Name = name });
		}

		private class DummyLogger : IPerfLogger<UserDto>
		{
			public void Error(string errorText, Exception exception)
			{

			}

			public void Warn(string message, string newLine, string stackTrace)
			{

			}
		}
	}


}