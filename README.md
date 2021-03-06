# EvolDB

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/Monbsoft/EvolDB/blob/master/LICENSE)
![.NET Core](https://github.com/Monbsoft/EvolDB/workflows/.NET%20Core/badge.svg)

EvolDB is a simple database migration tool with .Net Core. It is a tool to help you design evolutionary databases. It is inspired by [Evolve](https://github.com/lecaillon/Evolve) and [Flyway](https://flywaydb.org/).

## Commands

- _init_ : creates a migration repository.
- _commit_ : creates a commit.
- _push_ : Updates remote commits using local commits.
- _log_ : Show log commits.

```
evoldb commit V1_0_0_1__init.n1sql
```

## Getting Started

Create the data repository called _mydata_.

```
evoldb init mydata
```

Configure the config file called _config.json_.

```
{
  "ConnectionType": "Couchbase",
  "COUCHBASE_CONNECTIONSTRING": "couchbase://127.0.0.1",
  "COUCHBASE_BUCKET": <REPLACE-WITH-BUCKET>,
  "COUCHBASE_USERNAME": <REPLACE-WITH-USERNAME>,
  "COUCHBASE_PASSWORD": <REPLACE-WITH-PASSWORD>
}
```

Create the first commit called _V1_0_0_0\_\_init.n1ql_.

```
evoldb commit V1_0_0_0__init.n1ql
```

Update database with commits.

```
evoldb push
```

## Supported Databases

| Couchbase                              | SQLite                           |
| -------------------------------------- | -------------------------------- |
| ![Couchbase](doc/images/couchbase.png) | ![SQLite](doc/images/sqlite.png) |

## Community

- Ask questions by [opening an issue on GitHub](https://github.com/Monbsoft/EvolDB/issues).

## Licence

This project is licensed under the [MIT license](https://github.com/dotnet/orleans/blob/master/LICENSE).

## Credits

- P. Sadalage, M. Fowler, ["Evolutionary Database Design"](https://www.martinfowler.com/articles/evodb.html#YouDontNeedAnArmyOfDbas).
- [lecaillon / Evolve](https://github.com/lecaillon/Evolve), Database migration tool for .NET and .NET Core projects. Inspired by Flyway.
- [Boxfuse / Flyway](https://flywaydb.org/), Flyway by Boxfuse • Database Migrations Made Easy.
- [DbUp / DbUp](https://github.com/DbUp/DbUp), DbUp is a .NET library that helps you to deploy changes to SQL Server databases. It tracks which SQL scripts have been run already, and runs the change scripts that are needed to get your database up to date.
- [ differentway / couchmove](https://github.com/differentway/couchmove), Java data migration tool for Couchbase.
