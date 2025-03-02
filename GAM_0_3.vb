Public Class GAM_0_3
    Implements IAudiologyReceptionForm

    Public Property AudiologyReception As AudiologyReception Implements IAudiologyReceptionForm.AudiologyReception
        Get
            Return MyAudiologyReception
        End Get
        Set(value As AudiologyReception)
            MyAudiologyReception = value
        End Set
    End Property

    Private Sub GAM_0_1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub IAudiologyReceptionForm_Show() Implements IAudiologyReceptionForm.Show
        Me.Show()
    End Sub

    Private Sub IAudiologyReceptionForm_Close() Implements IAudiologyReceptionForm.Close
        Me.Close()
    End Sub

    Public Overrides Function ToString() As String Implements IAudiologyReceptionForm.ToString
        Return Me.Text
    End Function

    Private Sub AudiologyReception1_Paint(sender As Object, e As PaintEventArgs) Handles MyAudiologyReception.Paint

    End Sub
End Class