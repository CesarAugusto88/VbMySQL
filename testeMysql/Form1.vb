Imports MySql.Data.MySqlClient

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conexao = New MySqlConnection("server=localhost;port=3306;database=gstec;uid=root;password='1234' ")
                conexao.Open()
                Label1.Text = "HELLO WORLD! CONEXÃO BEM SUCEDIDA BANCO GSTEC"         
            End Using
        Catch ex As Exception
            MsgBox("Falha na conexão: " + ex.Message)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopularCbCampos()
        DataGridView1.DataSource = GetLivros("")
        If DataGridView1.RowCount > 0 Then
            ConfigurarGrade()
        End If
    End Sub

    Private Sub PopularCbCampos()
        With CbCampos
            .Items.Clear()
            .Items.Add("Título")
            .Items.Add("Autor")
            .Items.Add("Preço até")
        End With
    End Sub

    Private Function GetLivros(filtro As String) As DataTable
        Dim sql = "Select id, titulo, autores, isbn, unitario, ativo from livros WHERE ativo = 'S'"
        Dim dt As New DataTable

        If filtro <> "" Then
            If CbCampos.Text = "Título" Then
                sql = sql & "AND titulo LIKE '%" & filtro & "%'"
            ElseIf CbCampos.Text = "Autor" Then
                sql = sql & "AND autores LIKE '%" & filtro & "%'"
            ElseIf CbCampos.Text = "Preço até" Then
                filtro = filtro.Replace(",", ".")
                If IsNumeric(filtro) Then
                    sql = sql & "AND unitario <= " & filtro
                Else
                    MsgBox("A procura precisa ser um número!")
                End If
            End If
        End If

        Try
            Using conexao = New MySqlConnection("server=localhost;port=3306;database=livros;uid=root;password='1234' ")
                conexao.Open()
                Using da = New MySqlDataAdapter(sql, conexao)
                    da.Fill(dt)
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Falha na conexão: " & ex.Message)
            dt = Nothing

        End Try

        Return dt

    End Function

    Private Sub ConfigurarGrade()

        With DataGridView1

            DataGridView1.DefaultCellStyle.Font = New Font("Arial", 9)
            DataGridView1.RowHeadersWidth = 25

            DataGridView1.Columns("id").HeaderText = "ID"
            DataGridView1.Columns("id").Visible = False

            DataGridView1.Columns("titulo").HeaderText = "Título"
            DataGridView1.Columns("titulo").Width = 350

            DataGridView1.Columns("autores").HeaderText = "Autores"
            DataGridView1.Columns("autores").Width = 230

            DataGridView1.Columns("isbn").HeaderText = "ISBN"
            DataGridView1.Columns("isbn").Width = 120
            DataGridView1.Columns("isbn").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns("isbn").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridView1.Columns("unitario").HeaderText = "Preço"
            DataGridView1.Columns("unitario").Width = 60
            DataGridView1.Columns("unitario").HeaderCell.Style.Alignment =
                DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns("unitario").DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight

            DataGridView1.Columns("ativo").HeaderText = "Ativo"
            DataGridView1.Columns("ativo").Width = 40
            DataGridView1.Columns("ativo").HeaderCell.Style.Alignment =
                DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns("ativo").DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter

        End With

    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        DataGridView1.DataSource = GetLivros(TxtProcurar.Text)
        If DataGridView1.RowCount > 0 Then
            ConfigurarGrade()
        End If
    End Sub
End Class
