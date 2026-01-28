Public Class GAM_0_4_AudF
    Implements IAudiologyReceptionForm

    Public Property AudiologyReception As AudiologyReception Implements IAudiologyReceptionForm.AudiologyReception
        Get
            Return MyAudiologyReception
        End Get
        Set(value As AudiologyReception)
            MyAudiologyReception = value
        End Set
    End Property

    Private Sub GAM_0_4_AudF_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub IAudiologyReceptionForm_Show() Implements IAudiologyReceptionForm.Show
        Show()
    End Sub

    Private Sub IAudiologyReceptionForm_Close() Implements IAudiologyReceptionForm.Close
        Close()
    End Sub

    Private Function IAudiologyReceptionForm_ToString() As String Implements IAudiologyReceptionForm.ToString
        Return Me.Text
    End Function

    Private Sub GamSpace16_Paint(sender As Object, e As PaintEventArgs) Handles GamSpace16.Paint

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub GamSpace12_Paint(sender As Object, e As PaintEventArgs) Handles GamSpace12.Paint

    End Sub
End Class