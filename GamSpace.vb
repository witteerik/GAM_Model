
Public Class GamSpace
    Inherits FlowLayoutPanel

    Public Property SpaceName As String = ""

    Public Property SpaceType As GamSpaceTypes

    Private _NeedsCleaning As Boolean = False

    Public Property NeedsCleaning As Boolean
        Get
            Return _NeedsCleaning
        End Get
        Set(value As Boolean)
            _NeedsCleaning = value
            Me.Invalidate()

            If Me.Parent IsNot Nothing Then Parent.Invalidate()

        End Set
    End Property

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

            If Me.Parent IsNot Nothing Then Parent.Invalidate()

        End Set
    End Property


    Private _IsReserved As Boolean = False

    Public Property IsReserved As Boolean
        Get
            Return _IsReserved
        End Get
        Set(value As Boolean)
            _IsReserved = value
            Me.Invalidate()

            If Me.Parent IsNot Nothing Then Parent.Invalidate()

        End Set
    End Property


    Private ReserevedPen As Pen
    'Private NeedsCleaningPen As Pen

    Public Sub New()
        ReserevedPen = New Pen(Brushes.Gray)
        ReserevedPen.Width = 4

        'ReserevedPen = New Pen(Brushes.Yellow)
        'ReserevedPen.Width = 4

    End Sub

    Private _IsAvailable As Boolean = True

    Public Function IsAvailable() As Boolean
        Dim retVal = GetIsAvallableValue()

        If retVal <> _IsAvailable Then
            Me.Invalidate()
            If Me.Parent IsNot Nothing Then Parent.Invalidate()
            _IsAvailable = retVal
        End If

        Return retVal
    End Function

    Private Function GetIsAvallableValue() As Boolean

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

        If Me.SpaceType = GamSpaceTypes.AHM Or Me.SpaceType = GamSpaceTypes.MHM Or Me.SpaceType = GamSpaceTypes.Samtalsrum Then

            If NeedsCleaning = True Then
                e.Graphics.FillRectangle(Brushes.Yellow, e.ClipRectangle)
            Else
                If _IsAvailable = True Then
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.ClipRectangle)
                Else
                    e.Graphics.FillRectangle(Brushes.LightPink, e.ClipRectangle)
                End If
            End If

        End If

        If IsClosed = True Then
            'Draws a cross across the control if closed
            Dim Pen = New Pen(Brushes.Gray, Math.Max(2, 0.1 * Math.Sqrt(e.ClipRectangle.Width ^ 2 + e.ClipRectangle.Height ^ 2)))
            e.Graphics.DrawLine(Pen, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height)
            e.Graphics.DrawLine(Pen, 0, e.ClipRectangle.Height, e.ClipRectangle.Width, 0)
        End If

        If IsReserved() = True Then
            e.Graphics.DrawRectangle(ReserevedPen, 6, 6, e.ClipRectangle.Width - 12, e.ClipRectangle.Height - 12)
        End If

        'If NeedsCleaning = True Then
        '    e.Graphics.DrawRectangle(NeedsCleaningPen, 2, 2, e.ClipRectangle.Width - 4, e.ClipRectangle.Height - 4)
        'End If

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

