using AccesoDatos.DataBase;
using Entidades.Usuarios;
using System;
using System.Data;

namespace LogicaNegocio.Usuarios
{
    public class ClsUsuarioLn
    {
        #region Variables Privadas
        private clsDataBase ObjDataBase = null;
        #endregion

        #region Metodo Index
        // Metodo para listar información y mostrarlos en pantalla
        
        public void Index(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new clsDataBase()
            {
                NombreTabla = "TBL_Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Index]",
                Scalar = false, // es false porque no va ser un INSERT, DELETE O UPDATE.
            };
            Ejecutar(ref ObjUsuario);
        }

        #endregion

        #region CRUD de Usuario

        public void Create(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new clsDataBase()
            {
                NombreTabla = "TBL_Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Create]",
                Scalar = true, // es false porque no va ser un INSERT, DELETE O UPDATE.
            };

            // Enviando parametro del sp
            ObjDataBase.DtParameters.Rows.Add(@"@Nombre", "17", ObjUsuario.Nombre);
            ObjDataBase.DtParameters.Rows.Add(@"@Apellido1", "17", ObjUsuario.Apellido1);
            ObjDataBase.DtParameters.Rows.Add(@"@Apellido2", "17", ObjUsuario.Apellido2);
            ObjDataBase.DtParameters.Rows.Add(@"@FechaNacimiento", "13", ObjUsuario.FechaNacimiento);
            ObjDataBase.DtParameters.Rows.Add(@"@Estado", "1", ObjUsuario.Estado);

            Ejecutar(ref ObjUsuario);
        }
        public void Read(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new clsDataBase()
            {
                NombreTabla = "TBL_Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Read]",
                Scalar = false, // es false porque no va ser un INSERT, DELETE O UPDATE.
            };

            ObjDataBase.DtParameters.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);

            Ejecutar(ref ObjUsuario);
        }
        public void Update(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new clsDataBase()
            {
                NombreTabla = "TBL_Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Update]",
                Scalar = true, // es false porque no va ser un INSERT, DELETE O UPDATE.
            };

            ObjDataBase.DtParameters.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);
            ObjDataBase.DtParameters.Rows.Add(@"@Nombre", "17", ObjUsuario.Nombre);
            ObjDataBase.DtParameters.Rows.Add(@"@Apellido1", "17", ObjUsuario.Apellido1);
            ObjDataBase.DtParameters.Rows.Add(@"@Apellido2", "17", ObjUsuario.Apellido2);
            ObjDataBase.DtParameters.Rows.Add(@"@FechaNacimiento", "13", ObjUsuario.FechaNacimiento);
            ObjDataBase.DtParameters.Rows.Add(@"@Estado", "1", ObjUsuario.Estado);

            Ejecutar(ref ObjUsuario);
        }
        public void Delete(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new clsDataBase()
            {
                NombreTabla = "TBL_Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Delete]",
                Scalar = true, // es false porque no va ser un INSERT, DELETE O UPDATE.
            };

            ObjDataBase.DtParameters.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);

            Ejecutar(ref ObjUsuario);
        }

        #endregion

        #region Metodos Privados

        private void Ejecutar(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase.CRUD(ref ObjDataBase);
            if (ObjDataBase.MensajeErrorDB == null)
            {
                if (ObjDataBase.Scalar)
                {
                    ObjUsuario.ValorScalar = ObjDataBase.ValorScalar;
                }
                else
                {
                    ObjUsuario.DtResultados = ObjDataBase.DsResults.Tables[0];
                    if (ObjUsuario.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjUsuario.DtResultados.Rows)
                        {
                            ObjUsuario.IdUsuario = Convert.ToByte(item["IdUsuario"].ToString());
                            ObjUsuario.Nombre = item["Nombre"].ToString();
                            ObjUsuario.Apellido1 = item["Apellido1"].ToString();
                            ObjUsuario.Apellido2 = item["Apellido2"].ToString();
                            ObjUsuario.FechaNacimiento = Convert.ToDateTime(item["FechaNacimiento"].ToString());
                            ObjUsuario.Estado = Convert.ToBoolean(item["Estado"].ToString());
                        }
                    }
                }
            }
            else
            {
                ObjUsuario.MensajeError = ObjDataBase.MensajeErrorDB;
            }
        }

        #endregion
    }
}
