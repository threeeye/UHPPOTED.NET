<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UsersFRM
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        CloseBTN = New Button()
        GroupBox1 = New GroupBox()
        CancelBTN = New Button()
        SaveBTN = New Button()
        Label3 = New Label()
        RoleBOX = New ComboBox()
        PassTXT = New TextBox()
        UserTXT = New TextBox()
        Label2 = New Label()
        Label1 = New Label()
        DataGridView1 = New DataGridView()
        EditBTN = New Button()
        DeleteBTN = New Button()
        GroupBox1.SuspendLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' CloseBTN
        ' 
        CloseBTN.Location = New Point(727, 192)
        CloseBTN.Name = "CloseBTN"
        CloseBTN.Size = New Size(61, 23)
        CloseBTN.TabIndex = 0
        CloseBTN.Text = "Close"
        CloseBTN.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(CancelBTN)
        GroupBox1.Controls.Add(SaveBTN)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(RoleBOX)
        GroupBox1.Controls.Add(PassTXT)
        GroupBox1.Controls.Add(UserTXT)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Location = New Point(593, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(195, 174)
        GroupBox1.TabIndex = 1
        GroupBox1.TabStop = False
        GroupBox1.Text = "New User"
        ' 
        ' CancelBTN
        ' 
        CancelBTN.Location = New Point(102, 136)
        CancelBTN.Name = "CancelBTN"
        CancelBTN.Size = New Size(75, 23)
        CancelBTN.TabIndex = 6
        CancelBTN.Text = "Cancel"
        CancelBTN.UseVisualStyleBackColor = True
        ' 
        ' SaveBTN
        ' 
        SaveBTN.Location = New Point(6, 136)
        SaveBTN.Name = "SaveBTN"
        SaveBTN.Size = New Size(75, 23)
        SaveBTN.TabIndex = 5
        SaveBTN.Text = "Save"
        SaveBTN.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(33, 89)
        Label3.Name = "Label3"
        Label3.Size = New Size(33, 15)
        Label3.TabIndex = 2
        Label3.Text = "Role:"
        ' 
        ' RoleBOX
        ' 
        RoleBOX.DropDownStyle = ComboBoxStyle.DropDownList
        RoleBOX.FormattingEnabled = True
        RoleBOX.Items.AddRange(New Object() {"Admin", "Config", "Monitor"})
        RoleBOX.Location = New Point(77, 86)
        RoleBOX.Name = "RoleBOX"
        RoleBOX.Size = New Size(100, 23)
        RoleBOX.TabIndex = 4
        ' 
        ' PassTXT
        ' 
        PassTXT.Location = New Point(77, 46)
        PassTXT.Name = "PassTXT"
        PassTXT.Size = New Size(100, 23)
        PassTXT.TabIndex = 3
        ' 
        ' UserTXT
        ' 
        UserTXT.Location = New Point(77, 16)
        UserTXT.Name = "UserTXT"
        UserTXT.Size = New Size(100, 23)
        UserTXT.TabIndex = 2
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(6, 46)
        Label2.Name = "Label2"
        Label2.Size = New Size(60, 15)
        Label2.TabIndex = 1
        Label2.Text = "Password:"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(3, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(68, 15)
        Label1.TabIndex = 0
        Label1.Text = "User Name:"
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToResizeColumns = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(12, 12)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.Size = New Size(575, 203)
        DataGridView1.TabIndex = 2
        ' 
        ' EditBTN
        ' 
        EditBTN.Location = New Point(660, 192)
        EditBTN.Name = "EditBTN"
        EditBTN.Size = New Size(61, 23)
        EditBTN.TabIndex = 3
        EditBTN.Text = "Edit"
        EditBTN.UseVisualStyleBackColor = True
        ' 
        ' DeleteBTN
        ' 
        DeleteBTN.Location = New Point(593, 192)
        DeleteBTN.Name = "DeleteBTN"
        DeleteBTN.Size = New Size(61, 23)
        DeleteBTN.TabIndex = 4
        DeleteBTN.Text = "Delete"
        DeleteBTN.UseVisualStyleBackColor = True
        ' 
        ' UsersFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 228)
        Controls.Add(DeleteBTN)
        Controls.Add(EditBTN)
        Controls.Add(DataGridView1)
        Controls.Add(GroupBox1)
        Controls.Add(CloseBTN)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "UsersFRM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Users"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents CloseBTN As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents RoleBOX As ComboBox
    Friend WithEvents PassTXT As TextBox
    Friend WithEvents UserTXT As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents CancelBTN As Button
    Friend WithEvents SaveBTN As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents EditBTN As Button
    Friend WithEvents DeleteBTN As Button
End Class
