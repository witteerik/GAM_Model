<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartupForm
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
        Launch_25A_Button = New Button()
        Launch_26A_Button = New Button()
        SuspendLayout()
        ' 
        ' Launch_25A_Button
        ' 
        Launch_25A_Button.Location = New Point(32, 31)
        Launch_25A_Button.Name = "Launch_25A_Button"
        Launch_25A_Button.Size = New Size(158, 23)
        Launch_25A_Button.TabIndex = 0
        Launch_25A_Button.Text = "Launch CAR modeller 25A"
        Launch_25A_Button.UseVisualStyleBackColor = True
        ' 
        ' Launch_26A_Button
        ' 
        Launch_26A_Button.Location = New Point(32, 71)
        Launch_26A_Button.Name = "Launch_26A_Button"
        Launch_26A_Button.Size = New Size(158, 23)
        Launch_26A_Button.TabIndex = 1
        Launch_26A_Button.Text = "Launch CAR modeller 26A"
        Launch_26A_Button.UseVisualStyleBackColor = True
        ' 
        ' StartupForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(377, 217)
        Controls.Add(Launch_26A_Button)
        Controls.Add(Launch_25A_Button)
        Name = "StartupForm"
        Text = "CAR Modeller Startup Window"
        ResumeLayout(False)
    End Sub

    Friend WithEvents Launch_25A_Button As Button
    Friend WithEvents Launch_26A_Button As Button
End Class
