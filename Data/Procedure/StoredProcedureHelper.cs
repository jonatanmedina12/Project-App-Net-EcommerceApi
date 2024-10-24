using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Procedure
{
    public class StoredProcedureHelper
    {
        private readonly ApplicationDbContext _context;

        public StoredProcedureHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(
           string storedProcedure,
           params SqlParameter[] parameters) where T : class, new()
        {
            var result = new List<T>();

            // Obtener la conexión del contexto
            var conn = _context.Database.GetDbConnection();

            try
            {
                // Abrir la conexión si está cerrada
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetros si existen
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                using var reader = await command.ExecuteReaderAsync();
                result = MapReaderToList<T>(reader);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    await conn.CloseAsync();
            }

            return result;
        }
        private List<T> MapReaderToList<T>(DbDataReader reader) where T : class, new()
        {
            var results = new List<T>();
            var properties = typeof(T).GetProperties();

            // Crear un diccionario de propiedades para búsqueda más rápida
            var propertyDict = properties.ToDictionary(
                prop => prop.Name.ToLower(),
                prop => prop
            );

            while (reader.Read())
            {
                var item = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // Obtener el nombre de la columna
                    var columnName = reader.GetName(i).ToLower();

                    // Buscar la propiedad correspondiente
                    if (propertyDict.TryGetValue(columnName, out PropertyInfo property))
                    {
                        if (!reader.IsDBNull(i))
                        {
                            var value = reader.GetValue(i);

                            try
                            {
                                // Manejar tipos especiales
                                if (property.PropertyType == typeof(bool))
                                {
                                    // Convertir enteros a booleanos si es necesario
                                    if (value is int intValue)
                                    {
                                        property.SetValue(item, intValue == 1);
                                    }
                                    else
                                    {
                                        property.SetValue(item, value);
                                    }
                                }
                                else if (property.PropertyType.IsEnum)
                                {
                                    // Manejar enums
                                    property.SetValue(item, Enum.Parse(property.PropertyType, value.ToString()));
                                }
                                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                                {
                                    // Manejar tipos nullables
                                    var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                                    var convertedValue = Convert.ChangeType(value, underlyingType);
                                    property.SetValue(item, convertedValue);
                                }
                                else
                                {
                                    // Conversión estándar
                                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                    property.SetValue(item, convertedValue);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(
                                    $"Error al convertir el valor '{value}' a tipo '{property.PropertyType}' " +
                                    $"para la propiedad '{property.Name}'", ex);
                            }
                        }
                    }
                }

                results.Add(item);
            }

            return results;
        }
    }
}
