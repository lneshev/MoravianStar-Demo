namespace MoravianStar_Demo.Common.DataAccess.Constants
{
    /// <summary>
    /// Contains constants related to naming of database objects like schemas, tables, views, etc.
    /// </summary>
    public static class DbSchemaConstants
    {
        #region Schemas
        public const string DboSchema = "dbo";
        #endregion

        #region Tables, Views and Synonyms
        public const string Client = "Client";
        public const string Address = "Address";
        public const string Vehicle = "Vehicle";
        public const string Language = "Language";
        public const string Block = "Block";
        public const string ClientVehicle = "ClientVehicle";
        #endregion

        #region Sequences
        #endregion
    }
}