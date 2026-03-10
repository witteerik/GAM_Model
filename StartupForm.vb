Public Class StartupForm
    Private Sub Launch25A_Button_Click(sender As Object, e As EventArgs) Handles Launch_25A_Button.Click

        Dim ModellerWindow As New Modeller25A
        ModellerWindow.Show()

    End Sub

    Private Sub Launch26A_Button_Click(sender As Object, e As EventArgs) Handles Launch_26A_Button.Click

        Dim ModellerWindow As New Modeller26A
        ModellerWindow.Show()

    End Sub
End Class