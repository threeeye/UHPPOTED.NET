Imports System.DirectoryServices.AccountManagement
Imports AccessControl.Enums
Public Class LoginFRM
    Private Sub LoginFRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'UserTXT.Text = Environment.UserName

        InitUserDB()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Admin
        Dim f As New MainFRM With {.Tag = PrivilegeLevel.Admin}
        f.Show()
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Monitor
        Dim f As New MainFRM With {.Tag = PrivilegeLevel.Monitor}
        f.Show()
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Config
        Dim f As New MainFRM With {.Tag = PrivilegeLevel.Config}
        f.Show()
        Close()
    End Sub



    Private Function ValidateADUser(username As String, password As String, groupName As String) As Boolean
        Try
            Using context As New PrincipalContext(ContextType.Domain)
                ' First, validate the credentials
                If Not context.ValidateCredentials(username, password) Then Return False

                ' Now check group membership
                Using user = UserPrincipal.FindByIdentity(context, username)
                    If user IsNot Nothing Then
                        Return user.IsMemberOf(context, IdentityType.Name, groupName)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("AD error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return False
    End Function

    Private Sub LoginBTN_Click(sender As Object, e As EventArgs) Handles LoginBTN.Click
        Dim username = UserTXT.Text.Trim()
        Dim password = PassTXT.Text

        Dim requiredGroup = "AQControl" ' 🔁 Change to your actual AD group

        If ValidateADUser(username, password, requiredGroup) Then
            MessageBox.Show("Login successful!", "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' TODO: Show main form
        Else
            MessageBox.Show("Access denied. Invalid credentials or not in the required group.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub CloseBTN_Click(sender As Object, e As EventArgs) Handles CloseBTN.Click
        Close()
    End Sub

    Dim UserDB As DataTable
    Private Sub InitUserDB()
        UserDB = New DataTable
        UserDB.Columns.Add("User")
        UserDB.Columns.Add("Pass")
        UserDB.Columns.Add("Privilege", GetType(PrivilegeLevel))

        UserDB.Rows.Add("admin", "Admin!", PrivilegeLevel.Admin)
        UserDB.Rows.Add("monitor", "monitor", PrivilegeLevel.Monitor)
        UserDB.Rows.Add("manage", "!manage", PrivilegeLevel.Config)
    End Sub
End Class