using System;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos.DataBase
{
    public class clsDataBase
    {
        #region Variables Privadas

        // Variables para la conexion a la base de datos

        private SqlConnection _objSqlConnection; // Para la conexion a la base de datos
        private SqlDataAdapter _objSqlDataAdapter; // Permite hacer una lectura de datos
        private SqlCommand _objSqlCommand; // Permite enviar comandos para actualizar, crear y borrar información
        private DataSet _dsResults; // Lista de tablas de la base de datos
        private DataTable _dtParameters; // Se construyen los parametros para pasarlos a los sp's
        private string _nombreTabla, _nombreSP,
            _mensajeErrorDB, // Para almacenar los errores que puedan presentarse en la BD
            _valorScalar, // Valor de retorno en un INSERT, UPDATE, DELETE
            _nombreDB;
        private bool _scalar;

        #endregion

        #region Variables Publicas

        // ENCAPSULANDO LAS VARIABLES DE CONEXION
        public SqlConnection ObjSqlConnection { get => _objSqlConnection; set => _objSqlConnection = value; }
        public SqlDataAdapter ObjSqlDataAdapter { get => _objSqlDataAdapter; set => _objSqlDataAdapter = value; }
        public SqlCommand ObjSqlCommand { get => _objSqlCommand; set => _objSqlCommand = value; }
        public DataSet DsResults { get => _dsResults; set => _dsResults = value; }
        public DataTable DtParameters { get => _dtParameters; set => _dtParameters = value; }
        public string NombreTabla { get => _nombreTabla; set => _nombreTabla = value; }
        public string NombreSP { get => _nombreSP; set => _nombreSP = value; }
        public string MensajeErrorDB { get => _mensajeErrorDB; set => _mensajeErrorDB = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public string NombreDB { get => _nombreDB; set => _nombreDB = value; }
        public bool Scalar { get => _scalar; set => _scalar = value; }

        #endregion

        #region Contructor

        public clsDataBase() {         
            DtParameters = new DataTable("SpParametros"); // Nombre del data table
            DtParameters.Columns.Add("Nombre");
            DtParameters.Columns.Add("TipoDato");
            DtParameters.Columns.Add("Valor");

            NombreDB = "DB_BasePruebas";
        }

        #endregion

        #region Metodos Privados
        // 'ref' Significa referencia, son como variables globales. Mantienen su valor
        private void CrearConexionBaseDatos(ref clsDataBase ObjDataBase)
        {
            switch (ObjDataBase.NombreDB)
            {
                case "DB_BasePruebas":
                    // Conexion a la base de datos
                    ObjDataBase.ObjSqlConnection = new SqlConnection(Properties.Settings.Default.cadenaConexion_DB_BasePruebas);
                    break;
                default: break;
            }
        }
        private void ValidarConexionBaseDatos(ref clsDataBase ObjDataBase) 
        {
            // El ObjDataBase ya tiene cargada la conexion a la base de datos. Esto se realizó en el metodo CrearConexionBaseDatos.
            // Al ser una variable de referencia (ref) conserva su valor)
            if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Closed)
            {
                ObjDataBase.ObjSqlConnection.Open();//Abre conexion
            }
            else
            {
                ObjDataBase.ObjSqlConnection.Close();// cierra conexion
                ObjDataBase.ObjSqlConnection.Dispose(); // lo quita de memoria
            }
        }
        private void AgregarParametros(ref clsDataBase ObjDataBase)
        {
            if (ObjDataBase.DtParameters != null)
            {
                SqlDbType TipoDatoSql = new SqlDbType(); // Para recorrer la tabla de parametros
                foreach (DataRow item in ObjDataBase.DtParameters.Rows)
                {
                    switch (item[1].ToString()) // Columna de TipoDato
                    {
                        case "1":
                            TipoDatoSql = SqlDbType.Bit;
                            break;
                        case "2":
                            TipoDatoSql = SqlDbType.TinyInt;
                            break;
                        case "3":
                            TipoDatoSql = SqlDbType.SmallInt;
                            break;
                        case "4":
                            TipoDatoSql = SqlDbType.Int;
                            break;
                        case "5":
                            TipoDatoSql = SqlDbType.BigInt;
                            break;
                        case "6":
                            TipoDatoSql = SqlDbType.Decimal;
                            break;
                        case "7":
                            TipoDatoSql = SqlDbType.SmallMoney;
                            break;
                        case "8":
                            TipoDatoSql = SqlDbType.Money;
                            break;
                        case "9":
                            TipoDatoSql = SqlDbType.Float;
                            break;
                        case "10":
                            TipoDatoSql = SqlDbType.Real;
                            break;
                        case "11":
                            TipoDatoSql = SqlDbType.Date;
                            break;
                        case "12":
                            TipoDatoSql = SqlDbType.Time;
                            break;
                        case "13":
                            TipoDatoSql = SqlDbType.SmallDateTime;
                            break;
                        case "14":
                            TipoDatoSql = SqlDbType.DateTime;
                            break;
                        case "15":
                            TipoDatoSql = SqlDbType.Char;
                            break;
                        case "16":
                            TipoDatoSql = SqlDbType.NChar;
                            break;
                        case "17":
                            TipoDatoSql = SqlDbType.VarChar;
                            break;
                        case "18":
                            TipoDatoSql = SqlDbType.NVarChar;
                            break;
                        default:
                            break;
                    }

                    if (ObjDataBase.Scalar)
                    {
                        if (item[2].ToString().Equals(string.Empty)) // Columna Valor
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSql).Value = DBNull.Value;
                        }
                        else
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSql).Value = item[2].ToString();
                        }
                    }
                    else
                    {
                        if (item[2].ToString().Equals(string.Empty)) // Columna Valor
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSql).Value = DBNull.Value;
                        }
                        else
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSql).Value = item[2].ToString();
                        }
                    }
                }
            }
        }
        private void PrepararConexionBaseDatos(ref clsDataBase ObjDataBase)
        {
            CrearConexionBaseDatos(ref ObjDataBase);
            ValidarConexionBaseDatos(ref ObjDataBase);
        }
        private void EjecutarDataAdapter(ref clsDataBase ObjDataBase)
        {
            // Para ejecutar el SP
            try
            {
                PrepararConexionBaseDatos(ref ObjDataBase);
                ObjDataBase.ObjSqlDataAdapter = new SqlDataAdapter(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection);
                ObjDataBase.ObjSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                AgregarParametros(ref ObjDataBase);
                ObjDataBase.DsResults= new DataSet();
                ObjDataBase.ObjSqlDataAdapter.Fill(ObjDataBase.DsResults, ObjDataBase.NombreTabla);
            }
            catch (Exception ex)
            {
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                if(ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }
        private void EjecutarCommand(ref clsDataBase ObjDataBase)
        {
            try
            {
                PrepararConexionBaseDatos(ref ObjDataBase);
                ObjDataBase.ObjSqlCommand = new SqlCommand(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                AgregarParametros(ref ObjDataBase);

                if (ObjDataBase.Scalar)
                {
                    ObjDataBase.ValorScalar = ObjDataBase.ObjSqlCommand.ExecuteScalar().ToString().Trim();
                }
                else
                {
                    ObjDataBase.ObjSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }
        #endregion

        #region Metodos Publicos

        public void CRUD(ref clsDataBase ObjDataBase)
        {
            if (ObjDataBase.Scalar)
            {
                EjecutarCommand(ref ObjDataBase);
            }
            else
            {
                EjecutarDataAdapter(ref ObjDataBase);
            }
        }

        #endregion
    }
}
