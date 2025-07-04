<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainFRM
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        DataGridView1 = New DataGridView()
        Button1 = New Button()
        ControllersBTN = New Button()
        CardsBTN = New Button()
        DoorsBTN = New Button()
        RichTextBox1 = New RichTextBox()
        UsersBTN = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(12, 176)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(1103, 345)
        DataGridView1.TabIndex = 0
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(959, 54)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 1
        Button1.Text = "Button1"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' ControllersBTN
        ' 
        ControllersBTN.Location = New Point(1040, 12)
        ControllersBTN.Name = "ControllersBTN"
        ControllersBTN.Size = New Size(75, 23)
        ControllersBTN.TabIndex = 2
        ControllersBTN.Text = "Controllers"
        ControllersBTN.UseVisualStyleBackColor = True
        ' 
        ' CardsBTN
        ' 
        CardsBTN.Location = New Point(959, 12)
        CardsBTN.Name = "CardsBTN"
        CardsBTN.Size = New Size(75, 23)
        CardsBTN.TabIndex = 3
        CardsBTN.Text = "Cards"
        CardsBTN.UseVisualStyleBackColor = True
        ' 
        ' DoorsBTN
        ' 
        DoorsBTN.Location = New Point(1040, 54)
        DoorsBTN.Name = "DoorsBTN"
        DoorsBTN.Size = New Size(75, 23)
        DoorsBTN.TabIndex = 4
        DoorsBTN.Text = "Doors"
        DoorsBTN.UseVisualStyleBackColor = True
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Location = New Point(12, 12)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(941, 158)
        RichTextBox1.TabIndex = 5
        RichTextBox1.Text = ""
        ' 
        ' UsersBTN
        ' 
        UsersBTN.Location = New Point(1040, 93)
        UsersBTN.Name = "UsersBTN"
        UsersBTN.Size = New Size(75, 23)
        UsersBTN.TabIndex = 6
        UsersBTN.Text = "Users"
        UsersBTN.UseVisualStyleBackColor = True
        ' 
        ' MainFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1127, 533)
        Controls.Add(UsersBTN)
        Controls.Add(RichTextBox1)
        Controls.Add(DoorsBTN)
        Controls.Add(CardsBTN)
        Controls.Add(ControllersBTN)
        Controls.Add(Button1)
        Controls.Add(DataGridView1)
        Name = "MainFRM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Access Control"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents ControllersBTN As Button
    Friend WithEvents CardsBTN As Button
    Friend WithEvents DoorsBTN As Button
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents UsersBTN As Button

End Class
