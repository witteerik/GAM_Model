
Public Class GamSpace
    Inherits FlowLayoutPanel

    Public Property SpaceName As String = ""

    Public Property SpaceType As GamSpaceTypes

    Public Property NeedsCleaning As Boolean = False

    ''' <summary>
    ''' The number of timer ticks that the space is in use
    ''' </summary>
    ''' <returns></returns>
    Public Property UseTicks As Long = 0

    ''' <summary>
    ''' The number of timer ticks that the space is not used
    ''' </summary>
    ''' <returns></returns>
    Public Property VoidTicks As Long = 0

    ''' <summary>
    ''' The number of persons that were ever in the space at one and the same time.
    ''' </summary>
    ''' <returns></returns>
    Public Property HighestNumberOfPersons As Integer = 0

    Private _IsClosed As Boolean = False
    Public Property IsClosed As Boolean
        Get
            Return _IsClosed
        End Get
        Set(value As Boolean)
            _IsClosed = value
            Me.Invalidate()
        End Set
    End Property

    Public Function IsAvaliable() As Boolean

        If IsClosed Then Return False

        If NeedsCleaning = True Then Return False

        Select Case SpaceType
            Case GamSpaceTypes.MHM, GamSpaceTypes.Samtalsrum, GamSpaceTypes.Enkät, GamSpaceTypes.AHM

                'Returns true only if there is no patient at the place
                For Each control In Me.Controls
                    If TypeOf control Is Patient Then Return False
                Next

                Return True

            Case Else
                'Return true, since these places have "unlimited" space
                Return True
        End Select

    End Function

    Public Function GetPresentPersonnel() As List(Of Personnel)

        Dim PersonnelList As New List(Of Personnel)

        'Returns true only if there is no patient at the place
        For Each control In Me.Controls
            If TypeOf control Is Personnel Then
                PersonnelList.Add(control)
            End If
        Next

        Return PersonnelList

    End Function

    Public Function SomeoneIsCleaning() As Boolean

        Dim PersonnelList = GetPresentPersonnel()

        For Each Personnel In PersonnelList

            If Personnel.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Rengöring Then Return True

        Next

        Return False

    End Function

    Public Sub ExtraPaint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        If e.ClipRectangle.Width = 0 Then Exit Sub
        If e.ClipRectangle.Height = 0 Then Exit Sub

        If IsClosed = True Then
            'Draws a cross across the control if closed
            Dim Pen = New Pen(Brushes.Gray, Math.Max(2, 0.1 * Math.Sqrt(e.ClipRectangle.Width ^ 2 + e.ClipRectangle.Height ^ 2)))
            e.Graphics.DrawLine(Pen, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height)
            e.Graphics.DrawLine(Pen, 0, e.ClipRectangle.Height, e.ClipRectangle.Width, 0)
        End If

        'Writing the space name on the control
        If SpaceName = "" Then

            e.Graphics.DrawString(SpaceType.ToString, New Font(Me.Font.FontFamily, 8), Brushes.BlueViolet, New RectangleF(0, e.ClipRectangle.Height - 15, e.ClipRectangle.Width, 25))

        Else

            e.Graphics.DrawString(SpaceName, New Font(Me.Font.FontFamily, 8), Brushes.BlueViolet, New RectangleF(0, e.ClipRectangle.Height - 15, e.ClipRectangle.Width, 25))
        End If

    End Sub

    Private Sub GamPlace_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        'Changing the value of IsClosed on double clicks
        IsClosed = Not IsClosed
    End Sub

End Class

Public Enum GamSpaceTypes
    Väntrum
    Samtalsrum
    Kö_AHM
    AHM
    Kö_AHM_kvalitetsbedömning
    Kö_MHM
    MHM
    Enkät
    Kö_Rådgivning
    Personalyta
    PersonalRum
    Journaldatorplatser
    Utgång
End Enum

