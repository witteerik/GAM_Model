Imports System.ComponentModel

Public Class VerticalWall
    Inherits Panel

    Private Const FixedPanelWidth As Integer = 5

    Public Sub New()

        BackColor = Color.DarkSlateGray
        Me.Width = FixedPanelWidth

    End Sub

    Public Shadows Property Size As Size
        Get
            Return New Size(FixedPanelWidth, MyBase.Size.Height)
        End Get
        Set(value As Size)
            MyBase.Size = New Size(FixedPanelWidth, value.Height)
        End Set
    End Property

    ' Override SetBoundsCore to prevent width modifications
    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        MyBase.SetBoundsCore(x, y, FixedPanelWidth, height, specified)
    End Sub

End Class

Public Class HorizontalWall
    Inherits Panel

    Private Const FixedPanelHeight As Integer = 5

    Public Sub New()

        BackColor = Color.DarkSlateGray
        Me.Height = FixedPanelHeight

    End Sub

    Public Shadows Property Size As Size
        Get
            Return New Size(MyBase.Size.Width, FixedPanelHeight)
        End Get
        Set(value As Size)
            MyBase.Size = New Size(value.Width, FixedPanelHeight)
        End Set
    End Property

    ' Override SetBoundsCore to prevent width modifications
    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        MyBase.SetBoundsCore(x, y, width, FixedPanelHeight, specified)
    End Sub

End Class

Public Class Table
    Inherits Panel

    Public Sub New()
        BackColor = Color.FromArgb(255, 224, 192)
        Size = New Size(33, 33)

    End Sub

End Class
