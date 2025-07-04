Imports Microsoft.Data.SqlClient

Public Class UsersFRM
    Dim UserDB As DataTable
    Private Sub UsersFRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetUsers()

        DataGridView1.DataSource = UserDB
    End Sub

    Private Sub GetUsers()
        UserDB = New DataTable

        Dim cmd As New SqlCommand
        Dim con As New SqlConnection(ConString)
        Dim da As New SqlDataAdapter


        Try
            con.Open()
            cmd.Connection = con

            cmd.CommandText = $"SELECT * FROM AQUsers"
            da.SelectCommand = cmd
            da.Fill(UserDB)
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            da.Dispose()
        End Try
    End Sub

    Private Sub CancelBTN_Click(sender As Object, e As EventArgs) Handles CancelBTN.Click
        UserTXT.Clear()
        PassTXT.Clear()
        RoleBOX.SelectedItem = Nothing
        'SaveBTN.Enabled = False
    End Sub

    Private Sub SaveBTN_Click(sender As Object, e As EventArgs) Handles SaveBTN.Click
        If UserTXT.Text IsNot Nothing AndAlso PassTXT.Text IsNot Nothing AndAlso RoleBOX.SelectedItem IsNot Nothing Then
            MsgBox(1)
        Else
            MsgBox("One of the following is missing: Username, Password or Role")
        End If
    End Sub

    Private Sub AddUser(username As String, password As String, role As String)
        Dim salt = GenerateSalt()
        Dim hash = ComputeHash(password, salt)

        Using conn As New SqlConnection(ConString)
            conn.Open()
            Dim cmd As New SqlCommand("INSERT INTO AQUsers (Username, PasswordHash, Salt, Role) VALUES (@u, @h, @s, @r)", conn)
            cmd.Parameters.AddWithValue("@u", username)
            cmd.Parameters.AddWithValue("@h", hash)
            cmd.Parameters.AddWithValue("@s", salt)
            cmd.Parameters.AddWithValue("@r", role)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function GenerateSalt() As String
        Dim saltBytes(16 - 1) As Byte ' 128 bits
        Security.Cryptography.RandomNumberGenerator.Fill(saltBytes)
        Return Convert.ToBase64String(saltBytes)
    End Function

    Private Function ComputeHash(password As String, salt As String) As String
        Dim combined = System.Text.Encoding.UTF8.GetBytes(salt & password)
        Dim hash = Security.Cryptography.SHA256.HashData(combined)
        Return Convert.ToBase64String(hash)
    End Function
End Class