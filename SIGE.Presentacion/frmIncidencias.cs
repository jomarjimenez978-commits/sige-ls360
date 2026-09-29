using System;
using System.Drawing;
using System.Windows.Forms;
using SIGE.Datos;
using SIGE.Entidades;
using SIGE.Negocio;

namespace SIGE.Presentacion
{
    public class frmIncidencias : Form
    {
        private readonly IncidenciaBL _bl = new IncidenciaBL();
        private readonly TextBox txtTitulo = new TextBox { Left = 100, Top = 15, Width = 300 };
        private readonly TextBox txtDescripcion = new TextBox { Left = 100, Top = 45, Width = 300, Height = 60, Multiline = true };
        private readonly ComboBox cboCategoria = new ComboBox { Left = 100, Top = 115, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cboPrioridad = new ComboBox { Left = 100, Top = 145, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnRegistrar = new Button { Text = "Registrar", Left = 100, Top = 178, Width = 100 };
        private readonly ComboBox cboEstado = new ComboBox { Left = 460, Top = 15, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox txtComentario = new TextBox { Left = 460, Top = 45, Width = 300 };
        private readonly Button btnEstado = new Button { Text = "Cambiar estado", Left = 460, Top = 75, Width = 120 };
        private readonly Button btnEscalar = new Button { Text = "Escalar", Left = 590, Top = 75, Width = 80 };
        private readonly Button btnAuto = new Button { Text = "Escalar vencidas", Left = 680, Top = 75, Width = 110 };
        private readonly DataGridView grid = new DataGridView
        {
            Left = 15, Top = 220, Width = 970, Height = 320, ReadOnly = true,
            AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        public frmIncidencias()
        {
            Text = "SIGE-LS360 - Incidencias";
            ClientSize = new Size(1000, 555);
            StartPosition = FormStartPosition.CenterScreen;
            AddLabel("Título:", 15, 18); AddLabel("Descripción:", 15, 48);
            AddLabel("Categoría:", 15, 118); AddLabel("Prioridad:", 15, 148);
            AddLabel("Estado:", 410, 18); AddLabel("Comentario:", 385, 48);
            cboPrioridad.Items.AddRange(new object[] { "Baja", "Media", "Alta", "Critica" });
            cboEstado.Items.AddRange(new object[] { "Abierta", "En proceso", "Resuelta", "Cerrada" });
            Controls.AddRange(new Control[] { txtTitulo, txtDescripcion, cboCategoria, cboPrioridad,
                btnRegistrar, cboEstado, txtComentario, btnEstado, btnEscalar, btnAuto, grid });
            btnRegistrar.Click += (s, e) => Ejecutar(Registrar);
            btnEstado.Click += (s, e) => Ejecutar(CambiarEstado);
            btnEscalar.Click += (s, e) => Ejecutar(EscalarManual);
            btnAuto.Click += (s, e) => Ejecutar(EscalarAuto);
            Load += (s, e) => Ejecutar(CargarDatos);
        }

        private void AddLabel(string texto, int x, int y) =>
            Controls.Add(new Label { Text = texto, Left = x, Top = y, Width = 85 });

        // Manejo centralizado de errores: la interfaz solo muestra mensajes entendibles.
        private void Ejecutar(Action accion)
        {
            try { accion(); }
            catch (ValidacionException ex) { MessageBox.Show(ex.Message, "Validación"); }
            catch (AccesoDatosException ex) { MessageBox.Show(ex.Message, "Error de datos"); }
        }

        private void CargarDatos()
        {
            cboCategoria.DataSource = _bl.ListarCategorias();
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "IdCategoria";
            cboPrioridad.SelectedIndex = 1;
            cboEstado.SelectedIndex = 1;
            Refrescar();
        }

        private void Refrescar() => grid.DataSource = _bl.Listar()
            .ConvertAll(i => new { i.Id, i.Titulo, i.Categoria, i.Prioridad, i.Estado, Nivel = i.NivelActual, i.Sucursal, i.FechaCreacion });

        private int IdSeleccionado()
        {
            if (grid.CurrentRow == null) throw new ValidacionException("Seleccione una incidencia de la lista.");
            return Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
        }

        private void Registrar()
        {
            var inc = new Incidencia
            {
                Titulo = txtTitulo.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                IdCategoria = cboCategoria.SelectedValue == null ? 0 : Convert.ToInt32(cboCategoria.SelectedValue),
                Prioridad = cboPrioridad.Text
            };
            int id = _bl.Registrar(inc);
            MessageBox.Show("Incidencia registrada. Ticket #" + id);
            txtTitulo.Clear(); txtDescripcion.Clear();
            Refrescar();
        }

        private void CambiarEstado()
        {
            _bl.CambiarEstado(IdSeleccionado(), cboEstado.Text, txtComentario.Text.Trim());
            txtComentario.Clear();
            Refrescar();
        }

        private void EscalarManual()
        {
            _bl.EscalarManual(IdSeleccionado(), txtComentario.Text.Trim());
            Refrescar();
        }

        private void EscalarAuto()
        {
            int n = _bl.EscalarAutomaticamente();
            MessageBox.Show(n + " incidencia(s) escalada(s).");
            Refrescar();
        }
    }
}
