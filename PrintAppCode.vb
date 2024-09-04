' To compile, use: "dotnet publish -c Release -r win-x86" in the folder where the main.vbproj and print.vb files are located together.
'!When adding a new library, it's IMPORTANT to remember that it also needs to be placed in the .vbproj file within <ItemGroup>.

Imports System
Imports System.Drawing.Printing
Imports System.IO
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Xml
Imports System.Windows.Forms

Module PrintAppV2
    ' Variables for text formatting
    Public fontName As String = "Arial"
    Public style As FontStyle = FontStyle.Regular
    Public fontSize As Integer = 8
    Public marginTop As Integer = 0
    Public marginLeft As Integer = 0
    Public format As New StringFormat()
    Public currentFont As Font
    Public yPosition As Integer
    Public printDoc As New PrintDocument()
    Dim rectangle As New Rectangle

    Public isFormatting As Boolean = False
    ' Stack to manage nested formats
    Public formatStack As New Stack(Of TextFormat)

    Sub Main(args As String())
        If args.Length > 0 Then
            Dim txtFilePath As String = args(0)
            If Not File.Exists(txtFilePath) Then
                Console.WriteLine("The file does not exist.")
                Return
            End If
            Dim documentContent As String = File.ReadAllText(txtFilePath)

            Try
                FormatTxt(documentContent)
            Catch ex As Exception
                Dim errorMessage As String = "Error: " & ex.Message
                File.WriteAllText("error.txt", errorMessage)
            End Try
        Else
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New AboutForm())
        End If
    End Sub

    Sub FormatTxt(documentContent As String)
        Dim TOKENSTART As String = "#@-"
        Dim TOKENEND As String = "-#@"
        Dim lines As String() = documentContent.Split(Environment.NewLine)

        AddHandler printDoc.PrintPage, Sub(sender, e)
                                           yPosition = marginTop
                                           For Each line As String In lines
                                               ' Handle start of formatting
                                               If line.Trim().StartsWith(TOKENSTART) Then
                                                   ' Save current format on the stack
                                                   SaveCurrentFormat()
                                                   ' Set new format
                                                   line = line.Replace(TOKENSTART, "").Replace(" ", "")
                                                   Dim array As String() = line.Split(","c)
                                                   For Each attribute As String In array
                                                       Dim keyValue As String() = attribute.Split("="c)
                                                       Dim key As String = keyValue(0)
                                                       Dim value As String = keyValue(1)
                                                       GetFormat(key, value)
                                                   Next

                                                   isFormatting = True

                                                   ' Handle end of formatting
                                               ElseIf line.Trim().StartsWith(TOKENEND) Then
                                                   ' Restore format from the stack
                                                   RestorePreviousFormat()

                                                   ' Handle formatted lines
                                               ElseIf isFormatting = True And Not line.Trim().StartsWith(TOKENEND) And Not line.StartsWith(TOKENSTART) Then
                                                   AddTextFormat(line.Trim(), e)
                                               End If
                                           Next
                                           e.HasMorePages = False
                                       End Sub
        printDoc.Print()
    End Sub

    ' Method to save the current format on the stack
    Sub SaveCurrentFormat()
        Dim currentFormat As New TextFormat(fontName, style, fontSize, marginTop, marginLeft, format.Alignment, rectangle)
        formatStack.Push(currentFormat)
    End Sub

    ' Method to restore the format from the stack
    Sub RestorePreviousFormat()
        If formatStack.Count > 0 Then
            Dim previousFormat As TextFormat = formatStack.Pop()
            fontName = previousFormat.FontName
            style = previousFormat.Style
            fontSize = previousFormat.FontSize
            marginTop = previousFormat.MarginTop
            marginLeft = previousFormat.MarginLeft
            format.Alignment = previousFormat.Alignment
            rectangle = previousFormat.RectangleValue
        Else
            isFormatting = False
        End If
    End Sub

    ' Method to set the format based on keys and values
    Sub GetFormat(key As String, value As String)
        Select Case key.ToLower()
            Case "fontname"
                fontName = value
            Case "style"
                style = DirectCast([Enum].Parse(GetType(FontStyle), value, True), FontStyle)
            Case "margintop"
                marginTop = Integer.Parse(value)
            Case "marginleft"
                marginLeft = Integer.Parse(value)
            Case "fontsize"
                fontSize = Integer.Parse(value)
            Case "content"
                Select Case value.ToLower()
                    Case "near"
                        format.Alignment = StringAlignment.Near
                    Case "center"
                        format.Alignment = StringAlignment.Center
                    Case "far"
                        format.Alignment = StringAlignment.Far
                End Select
            Case "rectangle"
                Dim rectangleValue = value.Replace("{", "").Replace("}", "").Replace(" ", "").Split(";"c)

                Dim dimensions = Array.ConvertAll(rectangleValue, Function(str) Convert.ToInt32(str))
                rectangle = New Rectangle(0, 0, dimensions(0), dimensions(1))
        End Select
    End Sub

    ' Method to add formatted text to the page
    Sub AddTextFormat(text As String, e As PrintPageEventArgs)
        currentFont = New Font(fontName, fontSize, style)

        If rectangle.Width > 0 AndAlso rectangle.Height > 0 Then
            Dim rectF As New RectangleF(marginLeft, yPosition, rectangle.Width, rectangle.Height)
            e.Graphics.DrawString(text, currentFont, Brushes.Black, rectF, format)
        Else
            e.Graphics.DrawString(text, currentFont, Brushes.Black, marginLeft, yPosition, format)
        End If
        If marginTop > 0 Then

            yPosition += currentFont.Height + marginTop
        End If
    End Sub

    ' Class to store text formats
    Public Class TextFormat
        Public Property FontName As String
        Public Property Style As FontStyle
        Public Property FontSize As Integer
        Public Property MarginTop As Integer
        Public Property MarginLeft As Integer
        Public Property Alignment As StringAlignment
        Public Property RectangleValue As Rectangle
        Public Sub New(fontName As String, style As FontStyle, fontSize As Integer, marginTop As Integer, marginLeft As Integer, alignment As StringAlignment, rectangle As Rectangle)
            Me.FontName = fontName
            Me.Style = style
            Me.FontSize = fontSize
            Me.MarginTop = marginTop
            Me.MarginLeft = marginLeft
            Me.Alignment = alignment
            Me.RectangleValue = rectangle
        End Sub
    End Class
End Module
