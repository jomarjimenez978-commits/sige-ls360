using System;
using System.Drawing;
using System.Windows.Forms;
using SIGE.Datos;
using SIGE.Negocio;

namespace SIGE.Presentacion
{
    // Formulario creado por código (sin diseñador) para que sea fácil de leer y modificar.
    public class frmLogin : Form
    {
        private readonly TextBox txtCorreo = new TextBox { Left = 120, Top = 30, Width = 220 };
        private readonly TextBox txtClave = new TextBox { Left = 120, Top = 70, Width = 220, UseSystemPasswordChar = true };
        private readonly Button btnEntrar = new Button { Text = "Entrar", Left = 120, Top = 110, Width = 100 };
        private readonly UsuarioBL _bl = new UsuarioBL();

        public frmLogin()
        {
            Text = "SIGE-LS360 - Iniciar sesión";
            ClientSize = new Size(380, 160);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Controls.Add(new Label { Text = "Correo:", Left = 20, Top = 33, Width = 90 });
            Controls.Add(new Label { Text = "Contraseña:", Left = 20, Top = 73, Width = 90 });
            Controls.AddRange(new Control[] { txtCorreo, txtClave, btnEntrar });
            AcceptButton = btnEntrar;
            btnEntrar.Click += btnEntrar_Click;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                _bl.IniciarSesion(txtCorreo.Text, txtClave.Text);
                Hide();
                using (var f = new frmIncidencias()) f.ShowDialog();
                Close();
            }
            catch (ValidacionException ex) { MessageBox.Show(ex.Message, "Datos incorrectos"); }
            catch (AccesoDatosException) { MessageBox.Show("No se pudo conectar con la base de datos."); }
        }
    }
}
