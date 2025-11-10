There are two DbContexts in this solution by which migrations can be added.

By "SystemContext" migrations for the system database can be added.
By "ClientContext" migrations for the empty and client databases can be added.

To add a migration for the system database, use the following command:
Add-Migration [MIGRATION_NAME] -Context SystemContext -OutputDir "Migrations\System"

To add a migration for the empty and client databases, use the following command:
Add-Migration [MIGRATION_NAME] -Context ClientContext -OutputDir "Migrations\Client"

IMPORTANT!
1. Normally, when adding a new entity or a reference to another system entity as a property or collection, a migration for the system database should be created.
But if this entity exist as a synonym in the client database, a migration for the client database should also be created! EF Core automatically detect deep
references and might generate undesired scripts for creating tables in client's database. If such undesired scripts are generated in the migration,
consider adding more synonyms in the client's DbContext until "Up" and "Down" methods in the migration become empty upon its generation. Then, if necessary,
add the scripts for adding the synonyms there.
2. If you have to write a custom code in a migration, always use hard-coded strings instead of constants when you have to name tables, columns, indexes and so on.
If a constant is used to name something and constant's value is changed in future, this will lead to silently modifying all previous migrations where this constant
has been used. This probably will lead to errors for missing objects upon database creating or updating.